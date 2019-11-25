using Debugosaurus.BigUnits.Tests.TestTypes;
using Debugosaurus.BigUnits.Framework;
using Xunit;
using Shouldly;
using System;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        [Theory]
        [MemberData(nameof(PublicClasses.Data), MemberType=typeof(PublicClasses))]
        public void ClassScopeCanProvideTestInstancesForConcreteClasses(Type testInstanceType)
        {
            GivenTheTestScopeIs(TestScopes.Class(testInstanceType));
            
            var result = WhenATestInstanceIsRequested(testInstanceType);

            ThenAConcreteTestInstanceIsProvided(
                result,
                testInstanceType);
        }

        protected void GivenTheTestScopeIs(ITestScope testScope)
        {
            SetDependency(testScope);
        }

        protected object WhenATestInstanceIsRequested(Type testInstanceType)
        {
            return TestInstance.GetTestInstance(testInstanceType);
        }

        protected void ThenAConcreteTestInstanceIsProvided(
            object result,
            Type expectedType)
        {
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType(expectedType)
            );
        }
    }
}