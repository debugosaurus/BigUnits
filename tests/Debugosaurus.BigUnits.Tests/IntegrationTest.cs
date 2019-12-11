using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests
{
    public class IntegrationTest<T> where T : class
    {
        private BigUnitBuilder _bigUnitBuilder;

        protected IntegrationTest()
        {
            _bigUnitBuilder = new BigUnitBuilder()
                .WithTestScope(TestScopes.Namespace<T>())
                .WithDependencyProvider(new NotImplementedDependencyProvider());
        }

        private BigUnit BigUnit => _bigUnitBuilder.Build();

        protected T TestInstance => BigUnit.GetTestInstance<T>();

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