using System;
using Debugosaurus.BigUnits.Framework;

namespace Debugosaurus.BigUnits.Moq
{
    public static class BigUnitBuilderExtensions
    {
        public static BigUnitBuilder WithMoq(this BigUnitBuilder builder)
        {
            return builder.WithDependencyProvider(new MoqDependencyProvider());
        }
    }
}
