using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class ChildNamespaceScope : BaseTestScope
    {
        private readonly Type _type;
        private readonly string _namespacePrefix;

        public ChildNamespaceScope(Type type)
        {
            _type = type;
            _namespacePrefix = type.Namespace + ".";
        }

        protected override Type[] FetchTypesInScope()
        {
            return _type.Assembly
                .GetTypes()
                .Where(MatchesNamespace)
                .ToArray();
        }

        public override bool IsInScope(Type type)
        {
            return MatchesNamespace(type);
        }

        private bool MatchesNamespace(Type type)
        {
            return type.Namespace != null && type.Namespace.StartsWith(_namespacePrefix);
        }

        public override string ToString()
        {
            return $"child namespace of '{_type.Namespace}'";
        }
    }
}