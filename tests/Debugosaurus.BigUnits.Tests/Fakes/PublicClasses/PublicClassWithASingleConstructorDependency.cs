namespace Debugosaurus.BigUnits.Tests.Fakes.PublicClasses
{
    public class PublicClassWithASingleConstructorDependency
    {
        private IDependency dependency;

        public PublicClassWithASingleConstructorDependency(IDependency dependency) 
        {
            this.dependency = dependency;
        }
    }     
}