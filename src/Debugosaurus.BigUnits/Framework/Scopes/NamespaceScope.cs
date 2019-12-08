using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class NamespaceScope : ITestScope
    {
        private volatile Type[] _typesInScope;

        private readonly object SyncObj = new object();
        private readonly Type _type;

        public NamespaceScope(Type type)
        {
            _type = type;
        }

        public Type[] GetTypesInScope()
        {
            if (_typesInScope != null)
            {
                return _typesInScope;
            }

            lock (SyncObj)
            {
                if (_typesInScope == null)
                {
                    _typesInScope = _type.Assembly
                        .GetTypes()
                        .Where(MatchesNamespace)
                        .ToArray();
                }
            }

            return _typesInScope;
        }

        public bool IsInScope(Type type)
        {
            return MatchesNamespace(type);
        }

        private bool MatchesNamespace(Type type)
        {
            return
                type.Namespace == _type.Namespace ||
                (type.Namespace != null && type.Namespace.StartsWith(_type.Namespace + "."));
        }
    }
}