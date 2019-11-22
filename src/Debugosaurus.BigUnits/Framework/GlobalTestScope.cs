using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework
{
    public class GlobalTestScope : ITestScope
    {
        private static Type[] typesInScope;

        private static readonly object SyncObj = new object();

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
                            .ToArray();
                    }
                }
            }

            return typesInScope;
        }

        public bool IsInScope(Type type)
        {
            return true;
        }
    }
}