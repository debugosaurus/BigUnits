using System;
using System.Collections.Generic;

namespace Debugosaurus.BigUnits.Framework
{
    public class TypeCache
    {
        private readonly IDictionary<Type, object> _cache = new Dictionary<Type, object>();

        public object this[Type type]
        {
            get
            {
                _cache.TryGetValue(
                    type,
                    out var result);
                return result;
            }
        }

        public bool Contains(Type type)
        {
            return _cache.ContainsKey(type);
        }

        public void Add(
            Type type,
            object instance)
        {
            _cache[type] = instance;
        }
    }
}