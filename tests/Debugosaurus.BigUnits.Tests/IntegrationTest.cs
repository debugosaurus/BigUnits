using Debugosaurus.BigUnits.Framework;
using System;
using Xunit;

namespace Debugosaurus.BigUnits.Tests
{
    public class IntegrationTest<T> where T : class, new()
    {
        private readonly BigUnit bigUnit;

        protected IntegrationTest()
        {
            bigUnit = new BigUnit();
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