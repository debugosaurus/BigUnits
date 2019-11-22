using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests
{
    public abstract class UnitTest<T> where T : class
    {
        private BigUnitBuilder bigUnitBuilder;

        protected UnitTest() 
        {
            bigUnitBuilder = new BigUnitBuilder()
                .WithTestScope(TestScopes.Class<T>());
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
    }
}
