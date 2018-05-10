namespace HtmlDownLoader.FileWorkers
{
    using System;
    using System.IO;
    using System.Net;
    using HtmlAgilityPack;
    using Constraints;
    using Helpers;

    internal class FileDownLoader
    {
        private readonly ExtentionsConstraint _extentionsConstraint;
        private readonly ConsoleLogger _consoleLogger;
        private readonly UrlParser _parser;

        public FileDownLoader(UrlParser parser, ExtentionsConstraint extentionsConstraint, ConsoleLogger consoleLogger)
        {
            _extentionsConstraint = extentionsConstraint;
            _consoleLogger = consoleLogger;
            _parser = parser;
        }

        internal void DownLoadResources(HtmlDocument hap, Uri url)
        {
            _consoleLogger.LogProcess("Images downloading");
            DownloadResources(hap, url, "img", "src");
            _consoleLogger.LogProcess("Resources downloading");
            DownloadResources(hap, url, "link", "href");
            _consoleLogger.LogProcess("Scripts downloading");
            DownloadResources(hap, url, "script", "scr");
        }

        private void DownloadResources(HtmlDocument document, Uri pageUrl, string tagName, string attributeName)
        {
            var nodes = document.DocumentNode.SelectNodes($"//{tagName}");
            if (nodes == null) return;
            using (var client = new WebClient())
            {
                foreach (var htmlNode in nodes)
                {
                    var link = htmlNode.GetAttributeValue(attributeName, null);
                    if (!Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out var url)) continue;
                    if (!_extentionsConstraint.IsFileExtentionValid(link)) continue;
                    if (!url.IsAbsoluteUri)
                    {
                        url = new Uri(pageUrl, url);
                        link = url.OriginalString;
                    }

                    var newLink = _parser.GetFileName(url);
                    var file = new FileInfo(newLink);
                    file.Directory?.Create();
                    try
                    {
                        if (!File.Exists(newLink))
                        {
                            _consoleLogger.LogProcess($"\t{link}");
                            client.DownloadFile(link, newLink);
                        }

                        htmlNode.SetAttributeValue(attributeName, $"file://{newLink}");
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