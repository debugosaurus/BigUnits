using System;
using System.Collections.Generic;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public class OrTestScope : BaseTestScope
    {
        private readonly List<ITestScope> _testScopes = new List<ITestScope>();

        public OrTestScope(params ITestScope[] testScopes)
        {
            _testScopes.AddRange(testScopes);
        }

        protected override Type[] FetchTypesInScope()
        {
            return _testScopes
                .SelectMany(x => x.GetTypesInScope())
                .Distinct()
                .ToArray();
        }

        public override bool IsInScope(Type type)
        {
            return _testScopes.Any(x => x.IsInScope(type));
        }

        public override string ToString()
        {
            return string.Join(
                " or, ", 
                _testScopes.Select(x => x.ToString()));
        }
    }
}