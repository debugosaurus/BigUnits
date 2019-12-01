using System;
using System.Collections.Generic;

namespace Debugosaurus.BigUnits.Framework
{
    public class TypeCache
    {
        private IDictionary<Type, object> cache =  new Dictionary<Type, object>();

        public bool Contains(Type type)
        {
            return cache.ContainsKey(type);
        }

        public object this[Type type]
        {
            get
            {
                cache.TryGetValue(
                    type,
                    out var result);
                return result;
            }
        }

        public void Add(
            Type type,
            object instance) 
        {
            cache[type] = instance;
        }
    }
}