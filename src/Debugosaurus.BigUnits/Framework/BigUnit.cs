using System;
using System.Linq;
using Debugosaurus.BigUnits.Exceptions;
using Debugosaurus.BigUnits.Framework.Scopes;
using Debugosaurus.BigUnits.Framework.Construction;

namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnit
    {
        public BigUnit(
            ITestScope testScope,
            TestInstanceStrategy testInstanceStrategy,
            TestInstanceProvider testInstanceProvider) {

            TestScope = testScope;
            TestInstanceStrategy = testInstanceStrategy;
            TestInstanceProvider = testInstanceProvider;
        }

        public TTestInstance GetTestInstance<TTestInstance>()
        {
            return (TTestInstance) GetTestInstance(typeof(TTestInstance));
        }

        public object GetTestInstance(Type testInstanceType)
        {
            var buildAction = TestInstanceStrategy.GetBuildAction(testInstanceType);
            if(buildAction == null)
            {
                throw new BigUnitsException(
                    ExceptionMessages.TestInstanceNotInScope,
                    ("TestInstanceType" , testInstanceType));            
            }
            
            return TestInstanceProvider.CreateInstance(
                testInstanceType, 
                TestInstanceStrategy);
        }        

        public TestInstanceProvider TestInstanceProvider
        {
            get; set;
        }

        public TestInstanceStrategy TestInstanceStrategy
        {
            get; set;
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {            
            var buildActions = TestInstanceStrategy.GetBuildActionsInScope();
            var dependencies = buildActions
                .Where(x => x != null)
                .SelectMany(x => x.GetDependencyTypes())
                .Distinct()
                .Where(x => typeof(TDependency).IsAssignableFrom(x));

            if(!dependencies.Any())
            {
                throw new BigUnitsException(
                    ExceptionMessages.NotAValidDependencyType,
                    ("DependencyType", typeof(TDependency)));
            }
            
            TestInstanceProvider.SetDependency(dependency);
        }

        public TDependency GetDependency<TDependency>()
        {
            var buildActions = TestInstanceStrategy.GetBuildActionsInScope();
            var dependencies = buildActions
                .Where(x => x != null)
                .SelectMany(x => x.GetDependencyTypes())
                .Distinct()
                .Where(x => typeof(TDependency).IsAssignableFrom(x));

            if(!dependencies.Any())
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