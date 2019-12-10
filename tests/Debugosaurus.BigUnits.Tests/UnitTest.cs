using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests
{
    public abstract class UnitTest<T> where T : class
    {
        private BigUnitBuilder _bigUnitBuilder;

        protected UnitTest() 
        {
            _bigUnitBuilder = new BigUnitBuilder()
                .WithTestScope(TestScopes.Class<T>())
                .WithDependencyProvider(new NotImplementedDependencyProvider());
        }

        private BigUnit BigUnit => _bigUnitBuilder.Build();

        protected T TestInstance => BigUnit.GetTestInstance<T>();
    }
}