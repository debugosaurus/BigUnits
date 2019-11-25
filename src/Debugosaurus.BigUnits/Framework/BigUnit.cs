using System;
using System.Collections.Generic;

namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnit
    {
        private readonly IDictionary<Type, object> testInstances = new Dictionary<Type, object>();

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
            if(testInstances.ContainsKey(testInstanceType))
            {
                return testInstances[testInstanceType];
            }
            
            var result = TestInstanceProvider.CreateInstance(testInstanceType);
            testInstances.Add(
                testInstanceType,
                result);
            return result;
        }        

        public TestInstanceProvider TestInstanceProvider
        {
            get; set;
        }

        public void SetDependency<TDependency>(TDependency dependency)
        {
            TestInstanceProvider.SetDependency(dependency);
        }

        public ITestScope TestScope
        {
            get; set;
        }
    }
}