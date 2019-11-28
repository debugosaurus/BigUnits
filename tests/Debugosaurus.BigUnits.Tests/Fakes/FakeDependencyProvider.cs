using System;
using System.Collections.Generic;
using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Tests.TestTypes;

namespace Debugosaurus.BigUnits.Tests.Fakes
{    
    public class FakeDependencyProvider : IDependencyProvider
    {
        public IDictionary<Type, object> Audit = new Dictionary<Type, object>();

        public object GetDependency(Type type)
        {
            if(Audit.ContainsKey(type))
            {
                return Audit[type];
            }

            if(type == typeof(IDependency))
            {
                var result = new FakeDependency();
                Audit[type] = result;
                return result;
            }
            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDependency<>))
            {
                var dependencyType = type.GetGenericArguments()[0];
                var fakeDependencyType = typeof(FakeDependency<>).MakeGenericType(dependencyType);

                var result = Activator.CreateInstance(fakeDependencyType);

                Audit[type] = result;

                return result;
            }

            return null;
        }

        public class FakeDependency : IDependency {}
        public class FakeDependency<T> : IDependency<T> {}
    }
}