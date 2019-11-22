using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework
{
    public class NamespaceScope : ITestScope
    {
        private readonly string @namespace;

        private static Type[] typesInScope;

        private static readonly object SyncObj = new object();

        public NamespaceScope(string @namespace)
        {
            this.@namespace = @namespace;
        }

        public Type[] GetTypesInScope()
        {
            if(typesInScope == null)
            {
                lock(SyncObj)
                {
                    if(typesInScope == null)
                    {
                        typesInScope = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(x => x.GetTypes())
                            .Where(x => x.Namespace == @namespace)
                            .ToArray();
                    }
                }
            }

            return typesInScope;
        }

        public bool IsInScope(Type type)
        {
            return type.Namespace == @namespace;
        }
    }
}