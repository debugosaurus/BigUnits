namespace Debugosaurus.BigUnits.Tests
{
    public class UnitTest<T> : BaseUnitTest<T> where T : class
    {
        public UnitTest() : base(new NotImplementedDependencyProvider())
        {}
    }
}