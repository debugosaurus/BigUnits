using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public class TestInstanceProvider
    {
        private readonly IDependencyProvider _fakeProvider;
        private readonly TypeCache _typeCache;

        public TestInstanceProvider(
            IDependencyProvider fakeProvider,
            TypeCache typeCache)
        {
            _fakeProvider = fakeProvider;
            _typeCache = typeCache;
        }

        public object CreateInstance(
            Type type,
            TestInstanceStrategy strategy)
        {
            if (_typeCache.Contains(type))
            {
                return _typeCache[type];
            }

            object result;

            var buildAction = strategy.GetBuildAction(type);
            if (buildAction == null)
            {
                result = _fakeProvider.GetDependency(type);
            }
            else
            {
                var dependencies = buildAction.GetDependencyTypes();
                var parameters = dependencies
                    .Select(x => CreateInstance(x, strategy))
                    .ToArray();
                result = buildAction.Build(parameters);
            }

            _typeCache.Add(
                type,
                result);
            return result;
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {
            _typeCache.Add(
                typeof(TDependency),
                dependency);
        }
    }
}