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
        private IDependency dependency;
        
        public PublicClassWithASingleConstructorDependency(IDependency dependency) 
        {
            this.dependency = dependency;
        }
    }     

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

    public class PublicClassWithNestedPublicClasses
    {
        public class PublicClassWithDefaultConstructor
        {}

        public class PublicClassWithZeroArgConstructor
        {
            public PublicClassWithZeroArgConstructor() {}
        }    

        public class PublicClassWithASingleConstructorDependency
        {
            private IDependency dependency;

            public PublicClassWithASingleConstructorDependency(IDependency dependency) 
            {
                this.dependency = dependency;
            }
        }     

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

    public class PublicClasses
    {
        public static System.Collections.Generic.IEnumerable<object[]> Data =>
            new System.Collections.Generic.List<object[]>
            {
                new object[] { typeof(PublicClassWithDefaultConstructor) },
                new object[] { typeof(PublicClassWithZeroArgConstructor) },
                new object[] { typeof(PublicClassWithASingleConstructorDependency) },
                new object[] { typeof(PublicClassWithMultipleConstructorDependencies) },

                new object[] { typeof(PublicClassWithNestedPublicClasses.PublicClassWithDefaultConstructor) },
                new object[] { typeof(PublicClassWithNestedPublicClasses.PublicClassWithZeroArgConstructor) },
                new object[] { typeof(PublicClassWithNestedPublicClasses.PublicClassWithASingleConstructorDependency) },
                new object[] { typeof(PublicClassWithNestedPublicClasses.PublicClassWithMultipleConstructorDependencies) }

            };
    } 
}