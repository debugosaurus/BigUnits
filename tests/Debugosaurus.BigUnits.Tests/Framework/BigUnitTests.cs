using Debugosaurus.BigUnits.Tests.Fakes;
using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        protected IDependencyProvider FakeDependencyProvider => new FakeDependencyProvider();

        protected void GivenTheDependencyProviderIs(IDependencyProvider dependencyProvider)
        {
            SetDependency(dependencyProvider);
        }

        protected void GivenTheTestScopeIs(ITestScope testScope)
        {
            SetDependency(testScope);
        }

        protected void WhenATestInstanceIsRequested<TTestInstance>(out TTestInstance result)
        {
            result = TestInstance.GetTestInstance<TTestInstance>();
        }
    }
}