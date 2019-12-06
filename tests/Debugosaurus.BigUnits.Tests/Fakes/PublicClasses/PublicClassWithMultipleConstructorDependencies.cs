using Debugosaurus.BigUnits.Tests.Fakes;

namespace Debugosaurus.BigUnits.Tests.Fakes.PublicClasses
{
    public class PublicClassWithMultipleConstructorDependencies
    {
        private IDependency<object> dependency1;
        private IDependency<string> dependency2;
        private IDependency<int> dependency3;

        public PublicClassWithMultipleConstructorDependencies(
            IDependency<object> dependency1, 
            IDependency<string> dependency2, 
            IDependency<int> dependency3) 
        {
            this.dependency1 = dependency1;
            this.dependency2 = dependency2;
            this.dependency3 = dependency3;
        }
    }
}