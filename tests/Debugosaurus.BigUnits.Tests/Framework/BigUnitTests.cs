using Debugosaurus.BigUnits.Exceptions;
using Debugosaurus.BigUnits.Tests.Fakes;
using Debugosaurus.BigUnits.Tests.Fakes.PublicClasses;
using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Framework.Scopes;
using Xunit;
using Shouldly;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        [Fact]
        public void ClassScopeCanProvideTestInstancesForConcreteClasses()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithMultipleConstructorDependencies>());
            
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            WhenATestInstanceIsRequested(
                typeof(PublicClassWithMultipleConstructorDependencies),
                out var result);

            ThenAConcreteTestInstanceIsProvided(
                result,
                typeof(PublicClassWithMultipleConstructorDependencies));

            ThenAllDependenciesAreMocked(
                result,
                typeof(PublicClassWithMultipleConstructorDependencies));
        }

        [Fact]
        public void SameTestInstanceIsRetrievedEachTimeItIsRequested()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            WhenATestInstanceIsRequested(
                typeof(PublicClassWithDefaultConstructor),
                out var firstResult);

            WhenATestInstanceIsRequested(
                typeof(PublicClassWithDefaultConstructor),
                out var secondResult);

            secondResult.ShouldBe(firstResult);
        }     

        [Fact]
        public void RequestingATestInstanceOutsideOfScopeCausesAnError()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            Action action  = () => WhenATestInstanceIsRequested(
                typeof(object),
                out _);

            var exception = action.ShouldThrow<BigUnitsException>();
            exception.Data["TestInstanceType"].ShouldBe(typeof(object));
        }

        [Fact]
        public void RequestingADependencyWhenTheCurrentScopeHasNoDependenciesCausesAnError()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            Action action = () => TestInstance.GetDependency<object>();
            var exception = action.ShouldThrow<BigUnitsException>();
            exception.Data["DependencyType"].ShouldBe(typeof(object));
        }

        [Fact]
        public void RequestingADependencyWhenTheDependencyIsNotValidInTheCurrentScopeCausesAnError()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithASingleConstructorDependency>());
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            Action action = () => TestInstance.GetDependency<RankException>();
            var exception = action.ShouldThrow<BigUnitsException>();
            exception.Data["DependencyType"].ShouldBe(typeof(RankException));
        }


        [Fact]
        public void SettingADependencyWhenTheDependencyIsNotValidInTheCurrentScopeCausesAnError()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithASingleConstructorDependency>());
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            Action action = () => TestInstance.SetDependency(new RankException());
            var exception = action.ShouldThrow<BigUnitsException>();
            exception.Data["DependencyType"].ShouldBe(typeof(RankException));
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