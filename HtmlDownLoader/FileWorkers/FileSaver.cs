namespace HtmlDownLoader.FileWorkers
{
    using System;
    using System.IO;
    using Helpers;

    internal class FileSaver
    {
        private readonly UrlParser _urlParser;
        private readonly ConsoleLogger _consoleLogger;

        public FileSaver(UrlParser urlParser, ConsoleLogger consoleLogger)
        {
            _urlParser = urlParser;
            _consoleLogger = consoleLogger;
        }

        internal void SavePage(string fileName, string page)
        {
            var file = new FileInfo(fileName);
            file.Directory?.Create();
            File.WriteAllText(fileName, page);
        }

        internal void PrepareDirectory(Uri url)
        {
            var fullDirectoryName = _urlParser.GetDirectoryName(url);
            _consoleLogger.LogProcess($"Directory preparing: {fullDirectoryName}");
            if (Directory.Exists(fullDirectoryName))
            {
                Directory.Delete(fullDirectoryName, true);
            }

            Directory.CreateDirectory(fullDirectoryName);
        }
    }
}