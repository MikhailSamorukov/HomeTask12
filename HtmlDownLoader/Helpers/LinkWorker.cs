namespace HtmlDownLoader.Helpers
{
    using System;
    using HtmlAgilityPack;
    using System.Collections.Generic;
    using Constraints;

    internal class LinkWorker
    {
        private readonly DomainConsraint _domainConsraint;
        private readonly UrlParser _parser;

        public LinkWorker(UrlParser parser, DomainConsraint domainConsraint)
        {
            _domainConsraint = domainConsraint;
            _parser = parser;
        }

        internal void SetRelativePaths(HtmlDocument document, Uri pageUrl)
        {
            var nodes = document.DocumentNode.SelectNodes("//a");
            if (nodes == null) return;

            foreach (var htmlNode in nodes)
            {
                var link = htmlNode.GetAttributeValue("href", null);

                if (!Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out var url)) continue;
                if (url.IsAbsoluteUri) continue;

                url = new Uri(pageUrl, url);
                htmlNode.SetAttributeValue("href", url.OriginalString);
            }
        }

        internal IEnumerable<Uri> GetAllLinksFromPage(HtmlDocument document)
        {
            var nodes = document.DocumentNode.SelectNodes("//a");
            var result = new List<Uri>();
            if (nodes == null) return result;
            foreach (var htmlNode in nodes)
            {
                var link = htmlNode.GetAttributeValue("href", null);
                if (!Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out var url)) continue;
                
                if (!_domainConsraint.IsDomainWidthValid(url)) continue;

                var newLink = _parser.GetFileName(url);
                htmlNode.SetAttributeValue("href", newLink);
                result.Add(url);
            }

            return result;
        }

    }
}