using Debugosaurus.BigUnits.Exceptions;
using Debugosaurus.BigUnits.Tests.Fakes;
using Debugosaurus.BigUnits.Tests.Fakes.PublicClasses;
using Debugosaurus.BigUnits.Framework;
using Xunit;
using Shouldly;
using System;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnit_ClassScope_Tests : BigUnitTests
    {
        [Fact]
        public void CanCreatePopulatedTestInstances()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            WhenATestInstanceIsRequested<PublicClassWithDefaultConstructor>(out var result);

            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<PublicClassWithDefaultConstructor>()
            );
        }

        [Fact]
        public void ProvidesTheSameTestInstanceEachTime()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            WhenATestInstanceIsRequested<PublicClassWithDefaultConstructor>(out var firstResult);
            WhenATestInstanceIsRequested<PublicClassWithDefaultConstructor>(out var secondResult);

            secondResult.ShouldBe(firstResult);
        }

        [Fact]
        public void CanRetrieveTheDependenciesThatWereUsedToBuildTheTestInstance()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithMultipleConstructorDependencies>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            WhenATestInstanceIsRequested<PublicClassWithMultipleConstructorDependencies>(out var result);

            var dependency1 = TestInstance.GetDependency<IDependency<One>>();
            var dependency2 = TestInstance.GetDependency<IDependency<Two>>();
            var dependency3 = TestInstance.GetDependency<IDependency<Three>>();

            result.ShouldSatisfyAllConditions(
                () => result.Dependency1.ShouldBe(dependency1),
                () => result.Dependency2.ShouldBe(dependency2),
                () => result.Dependency3.ShouldBe(dependency3)
            );
        }

        [Fact]
        public void CanRetrieveTheDependenciesThatAreGoingToBeUsedToBuildTheTestInstance()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithMultipleConstructorDependencies>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            var dependency1 = TestInstance.GetDependency<IDependency<One>>();
            var dependency2 = TestInstance.GetDependency<IDependency<Two>>();
            var dependency3 = TestInstance.GetDependency<IDependency<Three>>();

            WhenATestInstanceIsRequested<PublicClassWithMultipleConstructorDependencies>(out var result);

            result.ShouldSatisfyAllConditions(
                () => result.Dependency1.ShouldBe(dependency1),
                () => result.Dependency2.ShouldBe(dependency2),
                () => result.Dependency3.ShouldBe(dependency3)
            );
        }

        [Fact]
        public void ThrowsAnExceptionWhenAskedForATestInstanceOutsideOfTheCurrentScope()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            var exception = Should.Throw<BigUnitsException>(
                () => WhenATestInstanceIsRequested<object>(out _));
            exception.Data[ExceptionData.TestInstanceType].ShouldBe(typeof(object));
        }

        [Fact]
        public void ThrowsAnExceptionWhenAskedForADependencyWhereThereAreNoneInTheCurrentScope()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithDefaultConstructor>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            var exception = Should.Throw<BigUnitsException>(
                () => TestInstance.GetDependency<object>());
            exception.Data[ExceptionData.DependencyType].ShouldBe(typeof(object));
        }

        [Fact]
        public void ThrowsAnExceptionWhenAskedForADependencyOutsideOfTheCurrentScope()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithASingleConstructorDependency>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            var exception = Should.Throw<BigUnitsException>(
                () => TestInstance.GetDependency<RankException>());
            exception.Data[ExceptionData.DependencyType].ShouldBe(typeof(RankException));
        }


        [Fact]
        public void ThrowsAnExceptionWhenAttemptingToSetADependencyOutsideOfTheCurrentScope()
        {
            GivenTheTestScopeIs(TestScopes.Class<PublicClassWithASingleConstructorDependency>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            var exception = Should.Throw<BigUnitsException>(
                () => TestInstance.SetDependency(new RankException()));
            exception.Data[ExceptionData.DependencyType].ShouldBe(typeof(RankException));
        }        
    }
}