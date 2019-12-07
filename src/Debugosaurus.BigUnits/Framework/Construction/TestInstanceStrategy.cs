using System;
using System.Linq;
using System.Collections.Generic;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public class TestInstanceStrategy
    {
        private readonly ITestScope testScope;

        private readonly IConstructorStrategy constructorStrategy;

        private readonly IDictionary<Type,IBuildAction> cache = new Dictionary<Type,IBuildAction>();

        public TestInstanceStrategy(
            ITestScope testScope,
            IConstructorStrategy constructorStrategy)
        {
            this.testScope = testScope;
            this.constructorStrategy = constructorStrategy;
        }

        public IBuildAction GetBuildAction(Type type)
        {
            IBuildAction result;

            if(cache.TryGetValue(type, out result))
            {
                return result;
            }

            if(testScope.IsInScope(type))
            {
                var targetType = type;

                if(type.IsAbstract || type.IsInterface)
                {
                    var scopedImplementationType = testScope.GetTypesInScope()
                        .Where(x => !x.IsAbstract && !x.IsInterface && type.IsAssignableFrom(x))
                        .Take(1)
                        .SingleOrDefault();
                    if(scopedImplementationType != null)
                    {
                        targetType = scopedImplementationType;
                    }
                    else
                    {
                        return null;
                    }
                }

                var constructor = constructorStrategy.GetConstructor(targetType);  
                result = new ConstructorBuildAction(
                    type,
                    constructor);
            }

            cache.Add(type, result);
            return result;
        }

        public IBuildAction[] GetBuildActionsInScope()
        {
            return testScope
                .GetTypesInScope()
                .Select(x => GetBuildAction(x))
                .ToArray();
        }
    }
}