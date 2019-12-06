using System;
using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests.Fakes
{
    public class FakeDependencyProvider : IDependencyProvider
    {
        private readonly TypeCache typeCache = new TypeCache();

        public object GetDependency(Type type)
        {
            if(typeCache.Contains(type))
            {
                return typeCache[type];
            }

            if(type == typeof(IDependency))
            {
                var result = new FakeDependency();
                typeCache.Add(
                    type, 
                    result);
                return result;
            }
            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDependency<>))
            {
                var dependencyType = type.GetGenericArguments()[0];
                var fakeDependencyType = typeof(FakeDependency<>).MakeGenericType(dependencyType);

                var result = Activator.CreateInstance(fakeDependencyType);

                typeCache.Add(
                    type,
                    result);

                return result;
            }

            return null;
        }

        public class FakeDependency : IDependency {}
        public class FakeDependency<T> : IDependency<T> {}
    }
}