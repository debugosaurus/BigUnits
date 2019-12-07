using System;
using Debugosaurus.BigUnits.Exceptions;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnit
    {
        public BigUnit(
            ITestScope testScope,
            TestInstanceProvider testInstanceProvider) {

            TestScope = testScope;
            TestInstanceProvider = testInstanceProvider;
        }

        public TTestInstance GetTestInstance<TTestInstance>()
        {
            return (TTestInstance) GetTestInstance(typeof(TTestInstance));
        }

        public object GetTestInstance(Type testInstanceType)
        {
            if(!TestScope.IsInScope(testInstanceType))
            {
                throw new BigUnitsException(
                    ExceptionMessages.TestInstanceNotInScope,
                    ("TestInstanceType" , testInstanceType));
            }
            
            return TestInstanceProvider.CreateInstance(testInstanceType);
        }        

        public TestInstanceProvider TestInstanceProvider
        {
            get; set;
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {
            TestInstanceProvider.SetDependency(dependency);
        }

        public TDependency GetDependency<TDependency>()
        {
            var dependencyTypes = new HashSet<Type>();
            var typesInScope = TestScope.GetTypesInScope();
            foreach(var typeInScope in typesInScope.Where(x => !x.IsInterface && !x.IsAbstract))
            {
                var constructor = new GreedyConstructorStrategy().GetConstructor(typeInScope);
                foreach(var parameter in constructor.GetParameters())
                {
                    dependencyTypes.Add(parameter.ParameterType);
                }
            }

            var matchingTypes = dependencyTypes.Where(x => typeof(TDependency).IsAssignableFrom(x));
            if(!matchingTypes.Any())
            {
                throw new BigUnitsException(
                    ExceptionMessages.NotAValidDependencyType,
                    ("DependencyType", typeof(TDependency)));
            }

            return TestInstanceProvider.GetDependency<TDependency>();
        }

        public ITestScope TestScope
        {
            get; set;
        }
    }
}