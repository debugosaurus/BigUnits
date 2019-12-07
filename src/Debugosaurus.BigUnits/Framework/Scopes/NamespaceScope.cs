using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class NamespaceScope : ITestScope
    {
        private readonly Type type;

        private static Type[] typesInScope;

        private static readonly object SyncObj = new object();

        public NamespaceScope(Type type)
        {
            this.type = type;
        }

        public Type[] GetTypesInScope()
        {
            if(typesInScope == null)
            {
                lock(SyncObj)
                {
                    if(typesInScope == null)
                    {
                        typesInScope = type.Assembly
                            .GetTypes()
                            .Where(x => MatchesNamespace(type, x))
                            .ToArray();
                    }
                }
            }

            return typesInScope;
        }

        public bool IsInScope(Type type)
        {
            return MatchesNamespace(
                this.type, 
                type);
        }

        private static bool MatchesNamespace(Type x, Type y)
        {
            return
                x.Namespace == y.Namespace ||
                y.Namespace.StartsWith(x.Namespace + ".");
        }
    }
}