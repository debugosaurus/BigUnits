using System;

namespace Debugosaurus.BigUnits.Framework
{
    public class ClassTestScope : ITestScope
    {
        private readonly Type type;

        public ClassTestScope(Type type)
        {
            this.type = type;
        }

        public Type[] GetTypesInScope()
        {
            return new[]{ type };
        }

        public bool IsInScope(Type type)
        {
            return type == this.type;
        }
    }
}