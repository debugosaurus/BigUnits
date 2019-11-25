using Debugosaurus.BigUnits.Framework;
using System;
using System.Diagnostics;
using Xunit;

namespace Debugosaurus.BigUnits.Tests
{
    public class IntegrationTest<T> where T : class
    {
        private readonly BigUnit bigUnit;

        protected IntegrationTest()
        {
            bigUnit = new BigUnit(
                TestScopes.Namespace<T>(),
                new TestInstanceProvider(
                    TestScopes.Namespace<T>(),
                    new GreedyConstructorStrategy())
            );
        }

        protected T TestInstance
        {
            get
            {
                return bigUnit.GetTestInstance<T>();
            }
        }

        protected void SetDependency<TDependency>(TDependency dependency)
        {
            bigUnit.SetDependency(dependency);
        }
    }
}