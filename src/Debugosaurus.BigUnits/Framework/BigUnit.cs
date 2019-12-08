using System;
using System.Linq;
using Debugosaurus.BigUnits.Exceptions;
using Debugosaurus.BigUnits.Framework.Construction;

namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnit
    {
        private readonly TestInstanceProvider _testInstanceProvider;
        private readonly TestInstanceStrategy _testInstanceStrategy;

        public BigUnit(
            TestInstanceStrategy testInstanceStrategy,
            TestInstanceProvider testInstanceProvider)
        {
            _testInstanceStrategy = testInstanceStrategy;
            _testInstanceProvider = testInstanceProvider;
        }

        public TTestInstance GetTestInstance<TTestInstance>()
        {
            return (TTestInstance) GetTestInstance(typeof(TTestInstance));
        }

        public object GetTestInstance(Type testInstanceType)
        {
            var buildAction = _testInstanceStrategy.GetBuildAction(testInstanceType);
            if (buildAction == null)
            {
                throw new BigUnitsException(
                    ExceptionMessages.TestInstanceNotInScope,
                    ("TestInstanceType", testInstanceType));
            }

            return _testInstanceProvider.CreateInstance(
                testInstanceType,
                _testInstanceStrategy);
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {
            var buildActions = _testInstanceStrategy.GetBuildActionsInScope();
            var dependencies = buildActions
                .Where(x => x != null)
                .SelectMany(x => x.GetDependencyTypes())
                .Distinct()
                .Where(x => typeof(TDependency).IsAssignableFrom(x));

            if (!dependencies.Any())
            {
                throw new BigUnitsException(
                    ExceptionMessages.NotAValidDependencyType,
                    ("DependencyType", typeof(TDependency)));
            }

            _testInstanceProvider.SetDependency(dependency);
        }

        public TDependency GetDependency<TDependency>()
        {
            var buildActions = _testInstanceStrategy.GetBuildActionsInScope();
            var dependencies = buildActions
                .Where(x => x != null)
                .SelectMany(x => x.GetDependencyTypes())
                .Distinct()
                .Where(x => typeof(TDependency).IsAssignableFrom(x));

            if (!dependencies.Any())
            {
                throw new BigUnitsException(
                    ExceptionMessages.NotAValidDependencyType,
                    ("DependencyType", typeof(TDependency)));
            }

            return _testInstanceProvider.GetDependency<TDependency>();
        }
    }
}