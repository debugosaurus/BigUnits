using System;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public interface IBuildAction
    {
        Type Type { get; }

        Type[] GetDependencyTypes();

        object Build(params object[] dependencies);
    }
}