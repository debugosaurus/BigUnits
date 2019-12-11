using System;
using Debugosaurus.BigUnits.Framework;
using Debugosaurus.BigUnits.Moq;
using Moq;
using Shouldly;
using Xunit;

namespace Debugosaurus.BigUnits.Moq.Tests
{
    public class MoqDependencyProviderTests : UnitTest<MoqDependencyProvider>
    {
        [Fact]
        public void CanMockInterfaces()
        {
            var result = TestInstance.GetDependency(typeof(IDisposable)) as IDisposable;

            Should.NotThrow(() => Mock<IDisposable>.Get(result),
                "should be a Moq provided mock object");
            result.ShouldNotBeNull();
        }

        [Fact]
        public void CanMockNonSealedClasses()
        {
            var result = TestInstance.GetDependency(typeof(object)) as Object;

            Should.NotThrow(() => Mock<Object>.Get(result),
                "should be a Moq provided mock object");
            result.ShouldNotBeNull();
        }
    }
}
