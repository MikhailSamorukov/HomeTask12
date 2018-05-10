namespace HtmlDownLoader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using FileWorkers;
    using HtmlAgilityPack;
    using Helpers;
    using Constraints;

    public class DownloadCore
    {
        private readonly DepthConstraint _depthConstraint;
        private readonly ConsoleLogger _consoleLogger;
        private readonly UrlParser _urlParser;
        private readonly FileDownLoader _fileDownLoader;
        private readonly FileSaver _fileSaver;
        private readonly LinkWorker _linkWorker;
        private readonly string _startUrl;
        
        public DownloadCore(string url, string pathToCopy, int depthCosntraint, DomainWidth domainConsraint, IEnumerable<string> extentions)
        {
            _startUrl = url;
            _depthConstraint = new DepthConstraint(depthCosntraint);
            _consoleLogger = ConsoleLogger.CreateInstance();
            _urlParser = new UrlParser(pathToCopy);
            _fileDownLoader = new FileDownLoader(_urlParser, new ExtentionsConstraint(extentions), _consoleLogger);
            _fileSaver = new FileSaver(_urlParser, _consoleLogger);
            _linkWorker = new LinkWorker(_urlParser, new DomainConsraint(domainConsraint, url));
        }

        public void Process()
        {
            var url = new Uri(_startUrl);

            _fileSaver.PrepareDirectory(url);

            using (var client = new HttpClient())
            {
                ProcessPage(client, url);
                _consoleLogger.LogResult("Finished");
            }
        }

        private void ProcessPage(HttpClient client, Uri url, int depth = 0)
        {
            _consoleLogger.LogProcess($"Page processing: {url}");
            using (var response = client.GetAsync(url).Result)
            using (var content = response.Content)
            {
                var result = content.ReadAsStringAsync().Result;
                if (result == null) return;
                var fileName = _urlParser.GetFileName(url);
                var hap = new HtmlDocument();
                hap.LoadHtml(result);

                _fileDownLoader.DownLoadResources(hap, url);

                _linkWorker.SetRelativePaths(hap, url);

                var links = new List<Uri>();
                if (_depthConstraint.IsDepthValid(depth))
                {
                    links.AddRange(_linkWorker.GetAllLinksFromPage(hap));
                }

                _fileSaver.SavePage(fileName, hap.DocumentNode.OuterHtml);
                foreach (var link in links)
                {
                    var targetUrl = link;
                    try
                    {
                        if (!targetUrl.IsAbsoluteUri)
                        {
                            targetUrl = new Uri(url, targetUrl);
                        }

                        fileName = _urlParser.GetFileName(targetUrl);
                        if (!File.Exists(fileName))
                        {
                            ProcessPage(client, targetUrl, ++depth);
                        }
                    }
                    catch (Exception ex)
                    {
                        _consoleLogger.LogError(ex.Message);
                    }
                }
            }
        }
    }
}
