using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class NamespaceScope : ITestScope
    {
        private static volatile Type[] _typesInScope;

        private static readonly object SyncObj = new object();
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
                        .Where(x => MatchesNamespace(_type, x))
                        .ToArray();
                }
            }

            return _typesInScope;
        }

        public bool IsInScope(Type type)
        {
            return MatchesNamespace(
                _type,
                type);
        }

        private static bool MatchesNamespace(
            Type x,
            Type y)
        {
            return
                x.Namespace == y.Namespace ||
                y.Namespace.StartsWith(x.Namespace + ".");
        }
    }
}