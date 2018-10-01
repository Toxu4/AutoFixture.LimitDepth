using System.Linq;
using AutoFixture;

namespace Toxu4.AutoFixture.LimitDepth.Lib
{
    public static class AutoFixtureExtensions
    {
        public static Fixture WithLimitedDepth(this Fixture fixture, int maxDepth = 2)
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new LimitDepth(maxDepth));
            
            return fixture;
        }
    } 
}
