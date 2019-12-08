namespace Debugosaurus.BigUnits.Tests.Fakes.PublicClasses
{
    public class PublicClassWithConcreteNamespaceDependencies
    {
        public readonly PublicClassWithMultipleConstructorDependencies ConcreteDependency;

        public PublicClassWithConcreteNamespaceDependencies(PublicClassWithMultipleConstructorDependencies concreteDependency)
        {
            ConcreteDependency = concreteDependency;
        }
    }
}