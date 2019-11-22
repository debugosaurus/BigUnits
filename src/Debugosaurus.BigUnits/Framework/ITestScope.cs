using System;

namespace Debugosaurus.BigUnits.Framework
{
    public interface ITestScope
    {
        bool IsInScope(Type type);

        Type[] GetTypesInScope();
    }
}