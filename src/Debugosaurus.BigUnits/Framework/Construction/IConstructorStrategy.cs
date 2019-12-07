using System;
using System.Reflection;

namespace Debugosaurus.BigUnits.Framework.Construction
{
    public interface IConstructorStrategy
    {
        ConstructorInfo GetConstructor(Type type);
    }
}