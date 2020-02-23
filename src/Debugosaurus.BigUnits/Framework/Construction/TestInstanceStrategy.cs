using System;
using System.Collections.Generic;
using System.Linq;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public class TestInstanceStrategy
    {
        private readonly IDictionary<Type, IBuildAction> _cache = new Dictionary<Type, IBuildAction>();

        private readonly IConstructorStrategy _constructorStrategy;
        private readonly ITestScope _testScope;

        public TestInstanceStrategy(
            ITestScope testScope,
            IConstructorStrategy constructorStrategy)
        {
            _testScope = testScope;
            _constructorStrategy = constructorStrategy;
        }

        public IBuildAction GetBuildAction(Type type)
        {
            if (_cache.TryGetValue(type, out var result))
            {
                return result;
            }

            if (_testScope.IsInScope(type))
            {
                var targetType = type;

                if (type.IsAbstract || type.IsInterface)
                {
                    var scopedImplementationType = _testScope.GetTypesInScope()
                        .Where(x => !x.IsAbstract && !x.IsInterface && type.IsAssignableFrom(x))
                        .Take(1)
                        .SingleOrDefault();
                    if (scopedImplementationType != null)
                    {
                        targetType = scopedImplementationType;
                    }
                    else
                    {
                        return null;
                    }
                }

                if (type.IsEnum)
                {
                    return new ExplicitBuildAction(
                        type,
                        Enum.ToObject(type, 0));
                }

                var constructor = _constructorStrategy.GetConstructor(targetType);
                result = new ConstructorBuildAction(
                    type,
                    constructor);
            }

            _cache.Add(type, result);
            return result;
        }

        public IBuildAction[] GetBuildActionsInScope()
        {
            return _testScope
                .GetTypesInScope()
                .Select(GetBuildAction)
                .ToArray();
        }

        public ITestScope TestScope {
            get
            {
                return _testScope;
            }
        }
    }
}