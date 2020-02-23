using System;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public class ExplicitBuildAction : IBuildAction
    {
        private readonly Type _type;
        private readonly object _value;

        public ExplicitBuildAction(
            Type type,
            object value)
        {
            _type = type;
            _value = value;
        }

        public Type Type => _type;

        public object Build(params object[] dependencies)
        {
            return _value;
        }

        public Type[] GetDependencyTypes()
        {
            return Type.EmptyTypes;
        }
    }
}