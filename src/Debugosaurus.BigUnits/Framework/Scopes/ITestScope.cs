using System;

namespace Debugosaurus.BigUnits.Framework.Scopes
{
    public interface ITestScope
    {
        bool IsInScope(Type type);

        Type[] GetTypesInScope();
    }
}