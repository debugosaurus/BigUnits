using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests
{
    public class IntegrationTest<T> where T : class
    {
        private BigUnitBuilder bigUnitBuilder;

        protected IntegrationTest()
        {
            bigUnitBuilder = new BigUnitBuilder()
                .WithTestScope(TestScopes.Namespace<T>());
        }

        private BigUnit BigUnit
        {
            get
            {
                return bigUnitBuilder.Build();
            }
        }

        protected T TestInstance
        {
            get
            {
                return BigUnit.GetTestInstance<T>();
            }
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