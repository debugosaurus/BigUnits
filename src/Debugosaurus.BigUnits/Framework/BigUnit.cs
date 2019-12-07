using System;
using Debugosaurus.BigUnits.Exceptions;

namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnit
    {
        public BigUnit(
            ITestScope testScope,
            TestInstanceProvider testInstanceProvider) {

            TestScope = testScope;
            TestInstanceProvider = testInstanceProvider;
        }

        public TTestInstance GetTestInstance<TTestInstance>()
        {
            return (TTestInstance) GetTestInstance(typeof(TTestInstance));
        }

        public object GetTestInstance(Type testInstanceType)
        {
            if(!TestScope.IsInScope(testInstanceType))
            {
                throw new BigUnitsException(
                    ExceptionMessages.TestInstanceNotInScope,
                    ("TestInstanceType" , testInstanceType));
            }
            
            return TestInstanceProvider.CreateInstance(testInstanceType);
        }        

        public TestInstanceProvider TestInstanceProvider
        {
            get; set;
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {
            TestInstanceProvider.SetDependency(dependency);
        }

        public TDependency GetDependency<TDependency>()
        {
            return TestInstanceProvider.GetDependency<TDependency>();
        }

        public ITestScope TestScope
        {
            get; set;
        }
    }
}