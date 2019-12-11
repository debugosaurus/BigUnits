using System;
using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Tests
{
    public class NotImplementedDependencyProvider : IDependencyProvider
    {
        public object GetDependency(Type type)
        {
            throw new NotImplementedException();
        }
    }
}