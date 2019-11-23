using System;
using System.Collections.Generic;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework
{
    public class TestInstanceProvider
    {
        private readonly ITestScope testScope;

        private readonly IConstructorStrategy constructorStrategy;

        private readonly IDependencyProvider dependencyProvider;

        private readonly IDictionary<Type,object> dependencies = new Dictionary<Type, object>();

        public TestInstanceProvider(
            ITestScope testScope,
            IConstructorStrategy constructorStrategy,
            IDependencyProvider dependencyProvider)
        {
            this.testScope = testScope;
            this.constructorStrategy = constructorStrategy;
            this.dependencyProvider = dependencyProvider;
        }

        public object CreateInstance(Type type)
        {
            if(constructorStrategy == null)
            {
                throw new Exception(string.Join(";",dependencies.Keys));
            }

            var constructor = constructorStrategy.GetConstructor(type);

            if(constructor == null)
            {
                throw new Exception("Cannot construct " + type.FullName);
            }

            var args = constructor
                .GetParameters()
                .Select(x => GetConstructorArgument(x))
                .ToArray();

            return Activator.CreateInstance(
                type, 
                args);
        }

        private object GetConstructorArgument(System.Reflection.ParameterInfo parameterInfo)
        {
            if(dependencies.ContainsKey(parameterInfo.ParameterType))
            {
                return dependencies[parameterInfo.ParameterType];
            }

            if(testScope.IsInScope(parameterInfo.ParameterType)) 
            {
                if(!parameterInfo.ParameterType.IsInterface && !parameterInfo.ParameterType.IsAbstract)
                {
                    return CreateInstance(parameterInfo.ParameterType);
                }

                var scopedImplementationType = testScope.GetTypesInScope()
                    .Where(x => !x.IsAbstract && !x.IsInterface && parameterInfo.ParameterType.IsAssignableFrom(x))
                    .SingleOrDefault();

                if(scopedImplementationType != null) 
                {
                    return CreateInstance(scopedImplementationType);
                }
            }
            
            return dependencyProvider.GetDependency(parameterInfo.ParameterType);
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {
            dependencies.Add(typeof(TDependency),dependency);
        }
    }
}