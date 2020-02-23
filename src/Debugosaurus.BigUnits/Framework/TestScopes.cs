using System;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits.Framework
{
    public static class TestScopes
    {
        public static ITestScope Any()
        {
            return new GlobalTestScope();
        }

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
            return Namespace<T>(NamespaceOptions.IncludeChildNamespaces);
        }

        public static ITestScope Namespace<T>(NamespaceOptions options)
        {
            if(options == NamespaceOptions.ExactNamespaceOnly)
            {
                return new NamespaceScope(typeof(T));
            }
            else
            {
                return new OrTestScope(
                    new NamespaceScope(typeof(T)),
                    new ChildNamespaceScope(typeof(T)));
            }
        }
    }
}