namespace Debugosaurus.BigUnits.Tests.Fakes.PublicClasses
{
    public class PublicClassWithASingleConstructorDependency
    {
        private IDependency _dependency;

        public PublicClassWithASingleConstructorDependency(IDependency dependency) 
        {
            this._dependency = dependency;
        }
    }     
}