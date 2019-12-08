using System;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class ClassTestScope : ITestScope
    {
        private readonly Type _type;

        public ClassTestScope(Type type)
        {
            _type = type;
        }

        public Type[] GetTypesInScope()
        {
            return new[] {_type};
        }

        public bool IsInScope(Type type)
        {
            return type == _type;
        }
    }
}