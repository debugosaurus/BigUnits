using System;
using System.Reflection;

namespace Debugosaurus.BigUnits.Framework
{
    public interface IConstructorStrategy
    {
        ConstructorInfo GetConstructor(Type type);
    }
}