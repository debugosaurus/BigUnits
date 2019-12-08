using Debugosaurus.BigUnits.Exceptions;
using Debugosaurus.BigUnits.Tests.Fakes;
using Debugosaurus.BigUnits.Tests.Fakes.PublicClasses;
using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Framework.Scopes;
using Xunit;
using Shouldly;
using System;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        [Fact]
        public void ClassScopeCanProvideTestInstancesForConcreteClasses()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithMultipleConstructorDependencies>());
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            WhenATestInstanceIsRequested<PublicClassWithMultipleConstructorDependencies>(out var result);

            ThenAConcreteTestInstanceIsProvided(result);
            ThenTheDependencyIsMocked(
                result,
                x => x.Dependency1);
            ThenTheDependencyIsMocked(
                result,
                x => x.Dependency2);
            ThenTheDependencyIsMocked(
                result,
                x => x.Dependency3);                
        }

        [Fact]
        public void SameTestInstanceIsRetrievedEachTimeItIsRequested()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            WhenATestInstanceIsRequested<PublicClassWithDefaultConstructor>(out var firstResult);
            WhenATestInstanceIsRequested<PublicClassWithDefaultConstructor>(out var secondResult);

            secondResult.ShouldBe(firstResult);
        }     

        [Fact]
        public void RequestingATestInstanceOutsideOfScopeCausesAnError()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(new FakeDependencyProvider());

            Action action  = () => WhenATestInstanceIsRequested<object>(out _);

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

        protected void ThenTheDependencyIsMocked<TResult,TDependency>(
            TResult result, 
            Func<TResult, TDependency> dependency)
        {
            var dependencyProvider = GetDependency<IDependencyProvider>();

            var expected = dependency(result);
            var actual = dependencyProvider.GetDependency(typeof(TDependency));

            actual.ShouldBe(expected);
        }

        protected void GivenTheTestScopeIs(ITestScope testScope)
        {
            SetDependency(testScope);
        }

        protected void WhenATestInstanceIsRequested<TTestInstance>(out TTestInstance result)
        {
            result = TestInstance.GetTestInstance<TTestInstance>();
        }        

        protected void ThenAConcreteTestInstanceIsProvided<TTestInstance>(TTestInstance testInstance)
        {
            testInstance.ShouldSatisfyAllConditions(
                () => testInstance.ShouldNotBeNull(),
                () => testInstance.GetType().ShouldBe(typeof(TTestInstance))
            );
        }        
    }
}