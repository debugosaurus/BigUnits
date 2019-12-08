using System;
using System.Linq;
using System.Reflection;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public class ConstructorBuildAction : IBuildAction
    {
        private readonly ConstructorInfo _constructor;

        public ConstructorBuildAction(ConstructorInfo constructor)
        {
            _constructor = constructor;
        }

        public Type[] GetDependencyTypes()
        {
            return _constructor.GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();
        }

        public object Build(params object[] parameters)
        {
            return _constructor.Invoke(parameters);
        }
    }
}