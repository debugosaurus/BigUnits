namespace Debugosaurus.BigUnits.Tests.TestTypes
{
    public class PublicClassWithDefaultConstructor
    {}

    public class PublicClassWithZeroArgConstructor
    {
        public PublicClassWithZeroArgConstructor() {}
    }    

    public class PublicClassWithASingleConstructorDependency
    {
        public PublicClassWithASingleConstructorDependency(object dependency) {}
    }     

    public class PublicClassWithMultipleConstructorDependencies
    {
        public PublicClassWithMultipleConstructorDependencies(object dependency1, object dependency2, object dependency3) {}
    }

    public class PublicClasses
    {
        public static System.Collections.Generic.IEnumerable<object[]> Data =>
            new System.Collections.Generic.List<object[]>
            {
                new object[] { typeof(PublicClassWithDefaultConstructor) },
                new object[] { typeof(PublicClassWithZeroArgConstructor) },
                new object[] { typeof(PublicClassWithASingleConstructorDependency) },
                new object[] { typeof(PublicClassWithMultipleConstructorDependencies) }
            };
    } 
}