using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests
{
    public class BigUnitTest<T> : BaseBigUnitTest where T : class
    {
        protected BigUnitTest() : base(new NotImplementedDependencyProvider())
        {
            TestScope = TestScopes.Namespace<T>(NamespaceOptions.IncludeChildNamespaces);
        }

        protected T TestInstance => base.GetTestInstance<T>();
    }
}