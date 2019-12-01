using Debugosaurus.BigUnits.Tests.Fakes;
using Debugosaurus.BigUnits.Tests.TestTypes;
using Debugosaurus.BigUnits.Framework;
using Xunit;
using Shouldly;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        [Theory]
        [MemberData(nameof(PublicClasses.Data), MemberType=typeof(PublicClasses))]
        public void ClassScopeCanProvideTestInstancesForConcreteClasses(Type testInstanceType)
        {
            GivenTheTestScopeIs(TestScopes.Class(testInstanceType));
            
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            WhenATestInstanceIsRequested(
                testInstanceType,
                out var result);

            ThenAConcreteTestInstanceIsProvided(
                result,
                testInstanceType);

            ThenAllDependenciesAreMocked(
                result,
                testInstanceType);
        }

        [Theory]
        [MemberData(nameof(PublicClasses.Data), MemberType=typeof(PublicClasses))]
        public void SameTestInstanceIsRetrievedEachTimeItIsRequested(Type testInstanceType)
        {
            GivenTheTestScopeIs(TestScopes.Class(testInstanceType));
            
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            WhenATestInstanceIsRequested(
                testInstanceType,
                out var firstResult);

            WhenATestInstanceIsRequested(
                testInstanceType,
                out var secondResult);

            secondResult.ShouldBe(firstResult);
        }     

        protected void GivenTheDependencyProviderIs(IDependencyProvider dependencyProvider)
        {
            SetDependency(dependencyProvider);
        }

        protected void ThenAllDependenciesAreMocked(
            object result, 
            Type testInstanceType)
        {
            var dependencyProvider = GetDependency<IDependencyProvider>();

            foreach(var dependencyField in testInstanceType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var dependency = dependencyField.GetValue(result);
                var dependencyType = dependencyField.FieldType;

                dependency.ShouldNotBeNull();
                dependency.ShouldBe(dependencyProvider.GetDependency(dependencyType));
            }    
        }

        protected void GivenTheTestScopeIs(ITestScope testScope)
        {
            SetDependency(testScope);
        }

        protected void WhenATestInstanceIsRequested(
            Type testInstanceType,
            out object result)
        {
            result = TestInstance.GetTestInstance(testInstanceType);
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