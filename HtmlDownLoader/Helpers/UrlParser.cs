namespace HtmlDownLoader.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class UrlParser
    {
        private readonly string _pathToFileCopy;

        public UrlParser(string pathToFileCopy)
        {
            _pathToFileCopy = pathToFileCopy;
        }

        public string GetFileName(Uri url)
        {
            string fileName;
            if (url.IsAbsoluteUri)
            {
                fileName = url.LocalPath.Trim("/".ToCharArray()).Replace('/', '\\');
                fileName = (string.IsNullOrEmpty(fileName) ? "index" : fileName) + url.Query.Replace('?', '!');
            }
            else
            {
                fileName = url.OriginalString.Trim("/".ToCharArray()).Replace('/', '\\');
            }

            var rgx = new Regex(@"[#].*$");
            fileName = rgx.Replace(fileName, "");
            rgx = new Regex(@"[.]\w+$");
            if (!rgx.IsMatch(fileName))
            {
                fileName = (string.IsNullOrEmpty(fileName) ? "index" : fileName) + ".html"; ;
            }

            fileName = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));

            var directoryName = GetDirectoryName(url);
            return Path.Combine(directoryName, fileName);
        }

        public string GetDirectoryName(Uri url)
        {
            var directoryName = url.Host + url.LocalPath.Replace('/', '\\');
            return Path.Combine(_pathToFileCopy, directoryName);
        }
    }
}