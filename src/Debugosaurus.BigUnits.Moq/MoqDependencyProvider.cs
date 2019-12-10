
using System;
using System.Reflection;
using Debugosaurus.BigUnits.Framework;
using Moq;
using System.Linq;
namespace Debugosaurus.BigUnits.Moq
{
    public class MoqDependencyProvider : IDependencyProvider
    {
        private static readonly MethodInfo MockMethod = typeof (MockRepository).GetMethod(nameof(MockRepository.Create), Type.EmptyTypes);

        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        public object GetDependency(Type type)
        {
            var mockResult = (Mock) MockMethod
                .MakeGenericMethod(type)
                .Invoke(_mockRepository, null);

            return mockResult.Object;
        }
    }
}