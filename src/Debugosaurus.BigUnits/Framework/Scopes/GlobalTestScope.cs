using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class GlobalTestScope : ITestScope
    {
        private static volatile Type[] _typesInScope;

        private static readonly object SyncObj = new object();

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
                    _typesInScope = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .ToArray();
                }
            }

            return _typesInScope;
        }

        public bool IsInScope(Type type)
        {
            return true;
        }
    }
}