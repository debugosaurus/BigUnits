using System;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits.Framework
{
    public static class TestScopes
    {
        public static ITestScope Class<T>() where T : class
        {
            return Class(typeof(T));
        }

        public static ITestScope Class(Type type)
        {
            return new ClassTestScope(type);
        }

        public static ITestScope Namespace<T>()
        {
            return new NamespaceScope(typeof(T));
        }
    }
}