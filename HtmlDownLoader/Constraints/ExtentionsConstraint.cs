namespace HtmlDownLoader.Constraints
{
    using System.Collections.Generic;
    using System.Linq;

    internal class ExtentionsConstraint
    {
        private readonly IEnumerable<string> _extentions;

        public ExtentionsConstraint(IEnumerable<string> extentions)
        {
            _extentions = extentions;
        }

        public bool IsFileExtentionValid(string fullFileName)
        {
            return _extentions.Any(extention => fullFileName.EndsWith($".{extention}"));
        }
    }
}
