namespace Debugosaurus.BigUnits.Framework
{
    public static class TestScopes
    {
        public static ITestScope Class<T>() where T : class, new()
        {
            return new ClassTestScope();
        }
    }
}