using Debugosaurus.BigUnits.Tests.Fakes;

namespace Debugosaurus.BigUnits.Tests.Fakes.PublicClasses
{
    public class PublicClassWithMultipleConstructorDependencies
    {
        public IDependency<object> Dependency1 { get; }
        public IDependency<string> Dependency2 { get; }
        public IDependency<int> Dependency3 { get; }

        public PublicClassWithMultipleConstructorDependencies(
            IDependency<object> dependency1, 
            IDependency<string> dependency2, 
            IDependency<int> dependency3) 
        {
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
        }
    }
}