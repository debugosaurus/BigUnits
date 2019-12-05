using System;
using System.Reflection;
using System.Linq;

namespace Debugosaurus.BigUnits.Framework
{
    public class ConstructorBuildAction : IBuildAction
    {
        private readonly Type type;

        private readonly ConstructorInfo constructor;

        public ConstructorBuildAction(
            Type type,
            ConstructorInfo constructor)
        {
            this.type = type;
            this.constructor = constructor;
        }

        public Type[] GetDependencyTypes()
        {
            return constructor.GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();
        }

        public object Build(params object[] parameters)
        {
            return constructor.Invoke(parameters);
        }
    }
}