using System;

namespace Debugosaurus.BigUnits.Framework
{
    public interface IDependencyProvider
    {
        object GetDependency(Type type);
    }
}