using System;

namespace Debugosaurus.BigUnits.Framework
{
    public interface IBuildAction
    {
        Type[] GetDependencyTypes();

        object Build(params object[] dependencies);
    }
}