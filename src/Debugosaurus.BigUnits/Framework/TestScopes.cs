using System;

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
            return Namespace(typeof(T));
        }  

        public static ITestScope Namespace(Type type)
        {
            return new NamespaceScope(type.Namespace);
        }
    }
}