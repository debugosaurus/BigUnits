namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnit
    {
        public TTestInstance GetTestInstance<TTestInstance>() where TTestInstance : class, new()
        {
            return new TTestInstance();
        }

        public ITestScope TestScope
        {
            get; set;
        }
    }
}