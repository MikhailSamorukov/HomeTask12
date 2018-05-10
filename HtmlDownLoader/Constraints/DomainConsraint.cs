namespace HtmlDownLoader.Constraints
{
    using System;

    internal class DomainConsraint
    {
        private readonly DomainWidth _constraint;
        private readonly Uri _rootUrl;

        public DomainConsraint(DomainWidth constraint, string rootUrl)
        {
            _constraint = constraint;
            _rootUrl = new Uri(rootUrl);
        }

        public bool IsDomainWidthValid(Uri link)
        {
            var uriString = $"http://{link.OriginalString.TrimStart('/')}";
            if (Uri.TryCreate(uriString, UriKind.Absolute, out var crossSiteUrl) && crossSiteUrl.IsAbsoluteUri)
            {
                link = crossSiteUrl;
            }

            switch (_constraint)
            {
                case DomainWidth.Domain:
                    return !link.IsAbsoluteUri || link.Host.Equals(_rootUrl.Host);
                case DomainWidth.UnderLink:
                    return !link.IsAbsoluteUri || link.AbsolutePath.StartsWith(_rootUrl.AbsolutePath);
                case DomainWidth.Unlimited:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("type does not supported");
            }
        }
    }
}
