using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests
{
    public class BigUnitTest<T> : BaseBigUnitTest where T : class
    {
        protected BigUnitTest() : base(
            TestScopes.Namespace<T>(), 
            new NotImplementedDependencyProvider())
        {}

        protected T TestInstance => base.GetTestInstance<T>();
    }
}