using Debugosaurus.BigUnits.Exceptions;
using Debugosaurus.BigUnits.Tests.Fakes;
using Debugosaurus.BigUnits.Tests.Fakes.PublicClasses;
using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Framework.Scopes;
using Xunit;
using Shouldly;
using System;

namespace Debugosaurus.BigUnits.Tests.Framework
{
    public class BigUnitTests : IntegrationTest<BigUnit>
    {
        protected IDependencyProvider FakeDependencyProvider => new FakeDependencyProvider();

        protected void GivenTheDependencyProviderIs(IDependencyProvider dependencyProvider)
        {
            SetDependency(dependencyProvider);
        }

        protected void GivenTheTestScopeIs(ITestScope testScope)
        {
            SetDependency(testScope);
        }

        protected void WhenATestInstanceIsRequested<TTestInstance>(out TTestInstance result)
        {
            result = TestInstance.GetTestInstance<TTestInstance>();
        }
    }
}