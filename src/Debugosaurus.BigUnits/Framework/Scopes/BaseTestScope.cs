using System;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public abstract class BaseTestScope : ITestScope
    {
       private volatile Type[] _typesInScope;

        private readonly object SyncObj = new object();

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
                    _typesInScope = FetchTypesInScope();
                }
            }

            return _typesInScope;
        }

        protected abstract Type[] FetchTypesInScope();

        public abstract bool IsInScope(Type type);      
    }
}