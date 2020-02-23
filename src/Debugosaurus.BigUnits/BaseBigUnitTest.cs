using System;
using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits
{
    public class BaseBigUnitTest
    {
        private BigUnitBuilder _bigUnitBuilder;

        protected BaseBigUnitTest(IDependencyProvider dependencyProvider)
        {
            _bigUnitBuilder = new BigUnitBuilder()
                .WithDependencyProvider(dependencyProvider);
        }

        private BigUnit BigUnit => _bigUnitBuilder.Build();

        protected ITestScope TestScope
        {
            set 
            {
                _bigUnitBuilder.WithTestScope(value);
            }
        }

        protected TTestInstance GetTestInstance<TTestInstance>()
        {
            return BigUnit.GetTestInstance<TTestInstance>();
        }

        protected void SetDependency<TDependency>(TDependency dependency)
        {
            BigUnit.SetDependency(dependency);
        }

        protected TDependency GetDependency<TDependency>()
        {
            return BigUnit.GetDependency<TDependency>();
        }
    }
}