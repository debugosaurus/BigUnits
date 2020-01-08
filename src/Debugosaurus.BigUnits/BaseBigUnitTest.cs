using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits
{
    public class BaseBigUnitTest
    {
        private BigUnitBuilder _bigUnitBuilder;

        protected BaseBigUnitTest(
            ITestScope testScope,
            IDependencyProvider dependencyProvider)
        {
            _bigUnitBuilder = new BigUnitBuilder()
                .WithTestScope(testScope)
                .WithDependencyProvider(dependencyProvider);
        }

        private BigUnit BigUnit => _bigUnitBuilder.Build();

        protected TTestInstance GetTestInstance<TTestInstance>()
        {
            return BigUnit.GetTestInstance<TTestInstance>();
        }

        protected void SetDependency<TDependency>(TDependency dependency)
        {
            BigUnit.SetDependency(dependency);
        }

        protected TDependency GetDependency<TDependency>()
        {
            return BigUnit.GetDependency<TDependency>();
        }
    }
}