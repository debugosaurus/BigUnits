using Debugosaurus.BigUnits.Tests.TestTypes;
using Debugosaurus.BigUnits.Framework;
using Xunit;
using Shouldly;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class FakeDependencyProvider : IDependencyProvider
    {
        public IDictionary<Type, object> Audit = new Dictionary<Type, object>();

        public object GetDependency(Type type)
        {
            if(type == typeof(IDependency))
            {
                var result = new FakeDependency();
                Audit[type] = result;
                return result;
            }
            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDependency<>))
            {
                var dependencyType = type.GetGenericArguments()[0];
                var fakeDependencyType = typeof(FakeDependency<>).MakeGenericType(dependencyType);

                var result = Activator.CreateInstance(fakeDependencyType);

                Audit[type] = result;

                return result;
            }

            return null;
        }

        public class FakeDependency : IDependency {}
        public class FakeDependency<T> : IDependency<T> {}
    }

    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        [Theory]
        [MemberData(nameof(PublicClasses.Data), MemberType=typeof(PublicClasses))]
        public void ClassScopeCanProvideTestInstancesForConcreteClasses(Type testInstanceType)
        {
            var dependencyProvider = new FakeDependencyProvider();

            GivenTheTestScopeIs(TestScopes.Class(testInstanceType));
            GivenTheDependencyProviderIs(dependencyProvider);

            var result = WhenATestInstanceIsRequested(testInstanceType);

            ThenAConcreteTestInstanceIsProvided(
                result,
                testInstanceType);

            ThenAllDependenciesAreMocked(
                result,
                testInstanceType,
                dependencyProvider);
        }

        protected void GivenTheDependencyProviderIs(IDependencyProvider dependencyProvider)
        {
            SetDependency(dependencyProvider);
        }

        protected void ThenAllDependenciesAreMocked(
            object result, 
            Type testInstanceType,
            FakeDependencyProvider dependencyProvider)
        {
            foreach(var dependencyField in testInstanceType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var dependency = dependencyField.GetValue(result);
                var dependencyType = dependencyField.FieldType;

                dependency.ShouldNotBeNull();
                dependency.ShouldBe(dependencyProvider.Audit[dependencyType]);
            }    
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