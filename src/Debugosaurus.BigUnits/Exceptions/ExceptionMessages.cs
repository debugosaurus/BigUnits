namespace Debugosaurus.BigUnits.Exceptions
{
    public static class ExceptionMessages
    {
        public const string TestInstanceNotInScope =
            "The requested test-instance is not in the configured unit's scope";

        public const string NotAValidDependencyType =
            "The specified type is not a dependency of any classes within the configured unit's scope";
    }
}