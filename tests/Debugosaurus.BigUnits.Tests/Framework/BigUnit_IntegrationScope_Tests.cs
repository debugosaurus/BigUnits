

using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Tests.Fakes;
using Debugosaurus.BigUnits.Tests.Fakes.PublicClasses;
using Shouldly;
using Xunit;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnit_IntegrationScope_Tests : BigUnitTests
    {
        [Fact]
        public void CanCreatePopulatedTestInstances()
        {
            GivenTheTestScopeIs(TestScopes.Namespace<PublicClassWithConcreteNamespaceDependencies>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            WhenATestInstanceIsRequested<PublicClassWithConcreteNamespaceDependencies>(out var result);

            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<PublicClassWithConcreteNamespaceDependencies>()
            );
        }

        [Fact]
        public void CanRetrieveTheDependenciesThatWereUsedToBuildTheTestInstance()
        {
            GivenTheTestScopeIs(TestScopes.Namespace<PublicClassWithConcreteNamespaceDependencies>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            WhenATestInstanceIsRequested<PublicClassWithConcreteNamespaceDependencies>(out var result);

            var concreteClass = TestInstance.GetDependency<PublicClassWithMultipleConstructorDependencies>();

            result.ConcreteDependency.ShouldBe(concreteClass);
        }       

        [Fact]
        public void CanRetrieveTheDependenciesThatAreGoingToBeUsedToBuildTheTestInstance()
        {
            GivenTheTestScopeIs(TestScopes.Namespace<PublicClassWithConcreteNamespaceDependencies>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            var concreteClass = TestInstance.GetDependency<PublicClassWithMultipleConstructorDependencies>();

            WhenATestInstanceIsRequested<PublicClassWithConcreteNamespaceDependencies>(out var result);

            result.ConcreteDependency.ShouldBe(concreteClass);
        }

        [Fact]
        public void CanRetrieveTheDependenciesThatWereIndirectlyUsedToBuildTheTestInstance()
        {
            GivenTheTestScopeIs(TestScopes.Namespace<PublicClassWithConcreteNamespaceDependencies>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            WhenATestInstanceIsRequested<PublicClassWithConcreteNamespaceDependencies>(out var result);

            var dependency1 = TestInstance.GetDependency<IDependency<One>>();
            var dependency2 = TestInstance.GetDependency<IDependency<Two>>();
            var dependency3 = TestInstance.GetDependency<IDependency<Three>>();

            result.ShouldSatisfyAllConditions(
                () => result.ConcreteDependency.Dependency1.ShouldBe(dependency1),
                () => result.ConcreteDependency.Dependency2.ShouldBe(dependency2),
                () => result.ConcreteDependency.Dependency3.ShouldBe(dependency3)
            );
        }        

        [Fact]
        public void CanRetrieveTheDependenciesThaAreGoingToBeIndirectlyUsedToBuildTheTestInstance()
        {
            GivenTheTestScopeIs(TestScopes.Namespace<PublicClassWithConcreteNamespaceDependencies>());
            GivenTheDependencyProviderIs(FakeDependencyProvider);

            var dependency1 = TestInstance.GetDependency<IDependency<One>>();
            var dependency2 = TestInstance.GetDependency<IDependency<Two>>();
            var dependency3 = TestInstance.GetDependency<IDependency<Three>>();

            WhenATestInstanceIsRequested<PublicClassWithConcreteNamespaceDependencies>(out var result);

            result.ShouldSatisfyAllConditions(
                () => result.ConcreteDependency.Dependency1.ShouldBe(dependency1),
                () => result.ConcreteDependency.Dependency2.ShouldBe(dependency2),
                () => result.ConcreteDependency.Dependency3.ShouldBe(dependency3)
            );
        }          
    }
}