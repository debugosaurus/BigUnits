using Debugosaurus.BigUnits.Framework;
using System;
using Xunit;

namespace Debugosaurus.BigUnits.Tests
{
    public abstract class UnitTest<T> where T : class, new()
    {
        private readonly BigUnit bigUnit;

        protected UnitTest() 
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
