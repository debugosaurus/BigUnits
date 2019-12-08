namespace Debugosaurus.BigUnits.Tests.Fakes.PublicClasses
{
    public class PublicClassWithMultipleConstructorDependencies
    {
        public IDependency<One> Dependency1 { get; }
        public IDependency<Two> Dependency2 { get; }
        public IDependency<Three> Dependency3 { get; }

        public PublicClassWithMultipleConstructorDependencies(
            IDependency<One> dependency1, 
            IDependency<Two> dependency2, 
            IDependency<Three> dependency3) 
        {
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
        }
    }
}