using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework
{
    public class TestInstanceProvider
    {
        private readonly TypeCache typeCache;
        
        private readonly ITestScope testScope;

        private readonly IConstructorStrategy constructorStrategy;

        private readonly IDependencyProvider fakeProvider;

        public TestInstanceProvider(
            ITestScope testScope,
            IConstructorStrategy constructorStrategy,
            IDependencyProvider fakeProvider,
            TypeCache typeCache)
        {
            this.testScope = testScope;
            this.constructorStrategy = constructorStrategy;
            this.fakeProvider = fakeProvider;
            this.typeCache = typeCache;
        }

        public object CreateInstance(Type type)
        {
            if(typeCache.Contains(type))
            {
                return typeCache[type];
            }

            object result;

            if(testScope.IsInScope(type))
            {
                var targetType = type;

                if(type.IsAbstract || type.IsInterface)
                {
                    var scopedImplementationType = testScope.GetTypesInScope()
                        .Where(x => !x.IsAbstract && !x.IsInterface && type.IsAssignableFrom(x))
                        .SingleOrDefault();
                    if(scopedImplementationType != null)
                    {
                        targetType = scopedImplementationType;
                    }
                }

                var constructorInfo = constructorStrategy.GetConstructor(targetType);
                var constructorArgs = constructorInfo.GetParameters()
                    .Select(x => CreateInstance(x.ParameterType))
                    .ToArray();

                result = constructorInfo.Invoke(constructorArgs);

                typeCache.Add(
                    type,
                    result);
                return result;
            }
            else
            {
                result = fakeProvider.GetDependency(type);

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