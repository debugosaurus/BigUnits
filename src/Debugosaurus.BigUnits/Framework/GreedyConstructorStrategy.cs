using System;
using System.Linq;
using System.Reflection;

namespace Debugosaurus.BigUnits.Framework
{
    public class GreedyConstructorStrategy : IConstructorStrategy
    {
        public ConstructorInfo GetConstructor(Type type)
        {
            var rankedConstructors = type
                .GetConstructors()
                .Where(x => x.IsPublic)
                .OrderByDescending(x => x.GetParameters().Length);

            return rankedConstructors.FirstOrDefault();
        }
    }
}