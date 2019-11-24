using Debugosaurus.BigUnits.Tests;
using Debugosaurus.BigUnits.Framework;
using Xunit;
using Shouldly;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        [Fact]
        public void ClassScopeCanProvideTestInstancesForConcreteClasses()
        {
            GivenTheTestScopeIs(TestScopes.Class<object>());

            var result = WhenATestInstanceIsRequested<object>();

            ThenAConcreteTestInstanceIsProvided<object>(result);
        }

        protected void GivenTheTestScopeIs(ITestScope testScope)
        {
            TestInstance.TestScope = testScope;
        }

        protected TTestInstance WhenATestInstanceIsRequested<TTestInstance>() where TTestInstance : class, new()
        {
            return TestInstance.GetTestInstance<TTestInstance>();
        }

        protected void ThenAConcreteTestInstanceIsProvided<TTestInstance>(object result)
        {
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<TTestInstance>()
            );
        }
    }
}