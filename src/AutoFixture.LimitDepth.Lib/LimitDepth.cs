using AutoFixture.Kernel;

namespace AutoFixture.LimitDepth.Lib
{
    public class LimitDepth : ISpecimenBuilderTransformation
    {
        private readonly int _maxDepth;

        public LimitDepth(int maxDepth = 2)
        {
            _maxDepth = maxDepth;
        }

        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            return new DepthGuard(_maxDepth, builder);
        }
    }
}