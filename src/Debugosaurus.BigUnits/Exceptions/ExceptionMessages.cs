namespace Debugosaurus.BigUnits.Exceptions
{
    public static class ExceptionMessages
    {
        public const string TestInstanceNotInScope =
            "The requested test-instance is not in the configured unit's scope";

        public static string NotAValidDependencyType(
            string Type, 
            string Scope)
        {
            return $"The specified type '{Type}' is not a dependency of any classes within the scope: {Scope}";
        }
    }
}