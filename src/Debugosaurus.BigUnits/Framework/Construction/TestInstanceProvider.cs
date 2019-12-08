using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public class TestInstanceProvider
    {
        private readonly TypeCache typeCache;

        private readonly IDependencyProvider fakeProvider;

        public TestInstanceProvider(
            IDependencyProvider fakeProvider,
            TypeCache typeCache)
        {
            this.fakeProvider = fakeProvider;
            this.typeCache = typeCache;
        }

        public object CreateInstance(
            Type type,
            TestInstanceStrategy strategy)
        {
            if(typeCache.Contains(type))
            {
                return typeCache[type];
            }

            object result;

            var buildAction = strategy.GetBuildAction(type);
            if(buildAction == null)
            {
                result = fakeProvider.GetDependency(type);
            }
            else
            {
                var dependencies = buildAction.GetDependencyTypes();
                var parameters = dependencies
                    .Select(x => CreateInstance(x, strategy))
                    .ToArray();
                result = buildAction.Build(parameters);
            }

            typeCache.Add(
                type,
                result);
            return result;   
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {
            typeCache.Add(
                typeof(TDependency),
                dependency);
        }

        public TDependency GetDependency<TDependency>()
        {
            return (TDependency) typeCache[typeof(TDependency)];
        }
    }
}