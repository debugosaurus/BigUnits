using System;
using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests.Fakes
{
    public class FakeDependencyProvider : IDependencyProvider
    {
        private readonly TypeCache _typeCache = new TypeCache();

        public object GetDependency(Type type)
        {
            if(_typeCache.Contains(type))
            {
                return _typeCache[type];
            }

            if(type == typeof(IDependency))
            {
                var result = new FakeDependency();
                _typeCache.Add(
                    type, 
                    result);
                return result;
            }
            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDependency<>))
            {
                var dependencyType = type.GetGenericArguments()[0];
                var fakeDependencyType = typeof(FakeDependency<>).MakeGenericType(dependencyType);

                var result = Activator.CreateInstance(fakeDependencyType);

                _typeCache.Add(
                    type,
                    result);

                return result;
            }

            return null;
        }

        private class FakeDependency : IDependency {}

        private class FakeDependency<T> : IDependency<T> {}
    }
}