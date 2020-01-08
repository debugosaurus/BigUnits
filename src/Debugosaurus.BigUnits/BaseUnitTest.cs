using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits
{
    public abstract class BaseUnitTest<T> where T : class
    {
        private BigUnitBuilder _bigUnitBuilder;

        protected BaseUnitTest(IDependencyProvider dependencyProvider) 
        {
            _bigUnitBuilder = new BigUnitBuilder()
                .WithTestScope(TestScopes.Class<T>())
                .WithDependencyProvider(dependencyProvider);
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