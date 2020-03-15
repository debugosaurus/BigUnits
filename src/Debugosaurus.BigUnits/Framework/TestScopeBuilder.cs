using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits.Framework
{
    public enum NamespaceOptions
    {
        IncludeChildNamespaces,
        ExactNamespaceOnly
    }

    public class TestScopeBuilder
    {
        private ITestScope _testScope;

        public TestScopeBuilder Any()
        {
            _testScope = new GlobalTestScope();
            return this;
        }

        public TestScopeBuilder Class<T>()
        {
            _testScope = new ClassTestScope(typeof(T));
            return this;
        }

        public TestScopeBuilder Namespace<T>()
        {
            _testScope = new NamespaceScope(typeof(T));
            return this;
        }

        public TestScopeBuilder Namespace<T>(NamespaceOptions namespaceOptions) 
        {
            if(namespaceOptions == NamespaceOptions.ExactNamespaceOnly)
            {
                _testScope = new NamespaceScope(typeof(T));
            }
            else
            {
                _testScope = new OrTestScope(
                    new NamespaceScope(typeof(T)),
                    new ChildNamespaceScope(typeof(T)));
            }
            return this;
        }

        public ITestScope Build()
        {
            return _testScope;
        }
    }
}