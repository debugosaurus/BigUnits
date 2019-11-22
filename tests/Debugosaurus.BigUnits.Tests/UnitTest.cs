using Debugosaurus.BigUnits.Framework;
using System;
using Xunit;

namespace Debugosaurus.BigUnits.Tests
{
    public abstract class UnitTest<T> where T : class
    {
        private readonly BigUnit bigUnit;

        protected UnitTest() 
        {
            bigUnit = new BigUnit(
                TestScopes.Class<T>(),
                new TestInstanceProvider(
                    TestScopes.Class<T>(),
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
    }
}
