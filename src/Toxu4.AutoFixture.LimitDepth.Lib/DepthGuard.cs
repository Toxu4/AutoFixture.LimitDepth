using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using AutoFixture.Kernel;

namespace Toxu4.AutoFixture.LimitDepth.Lib
{
    internal class DepthGuard : ISpecimenBuilderNode
    {
        private readonly ThreadLocal<int> _depthsByThread = new ThreadLocal<int>(() => 0);

        private readonly int _maxDepth;
        private readonly ISpecimenBuilder _builder;

        public DepthGuard(int maxDepth, ISpecimenBuilder builder)
        {
            _maxDepth = maxDepth;
            _builder = builder;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (_depthsByThread.Value > _maxDepth && (request is PropertyInfo || request is FieldInfo))
            {
                return new OmitSpecimen();
            }

            if (request is SeededRequest)
            {
                _depthsByThread.Value++;
            }

            try
            {
                return _builder.Create(request, context);
            }
            finally
            {
                if (request is SeededRequest)
                {
                    _depthsByThread.Value--;
                }
            }
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return _builder;
        }

        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = new CompositeSpecimenBuilder(builders);

            return new DepthGuard(_maxDepth, composedBuilder);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}