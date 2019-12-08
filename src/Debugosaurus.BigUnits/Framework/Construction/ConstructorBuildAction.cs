using System;
using System.Linq;
using System.Reflection;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public class ConstructorBuildAction : IBuildAction
    {
        private readonly Type _type;

        private readonly ConstructorInfo _constructor;

        public ConstructorBuildAction(
            Type type,
            ConstructorInfo constructor)
        {
            _type = type;
            _constructor = constructor;
        }

        public Type Type => _type;

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