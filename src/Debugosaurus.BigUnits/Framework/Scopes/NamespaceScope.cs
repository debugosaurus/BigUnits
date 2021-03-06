using System;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class NamespaceScope : BaseTestScope
    {
        private readonly Type _type;

        public NamespaceScope(Type type)
        {
            _type = type;
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
            return type.Namespace == _type.Namespace;
        }

        public override string ToString()
        {
            return $"namespace equals '{_type.Namespace}'";
        }
    }
}