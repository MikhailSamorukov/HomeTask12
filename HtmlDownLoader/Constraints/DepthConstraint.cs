namespace HtmlDownLoader.Constraints
{
    internal class DepthConstraint
    {
        private readonly int _maxDepth;

        public DepthConstraint(int maxDepth)
        {
            _maxDepth = maxDepth;
        }

        public bool IsDepthValid(int currentDepth)
        {
            return currentDepth < _maxDepth;
        }
    }
}
