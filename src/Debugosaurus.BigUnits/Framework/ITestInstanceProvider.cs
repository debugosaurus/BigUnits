using System;

namespace Debugosaurus.BigUnits.Framework
{
    public interface ITestInstanceProvider
    {
        object CreateInstance(Type type);
        void SetDependency<TDependency>(TDependency dependency);
    }
}