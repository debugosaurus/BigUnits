using Debugosaurus.BigUnits.Framework.Construction;
using Debugosaurus.BigUnits.Framework.Scopes;

namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnitBuilder
    {
        private BigUnit _bigUnit;

        private IConstructorStrategy _constructorStrategy;

        private IDependencyProvider _dependencyProvider;
        private ITestScope _testScope;

        public BigUnitBuilder()
        {
            _testScope = new GlobalTestScope();
            _constructorStrategy = new GreedyConstructorStrategy();
        }

        public BigUnitBuilder WithTestScope(ITestScope testScope)
        {
            _bigUnit = null;
            _testScope = testScope;
            return this;
        }

        public BigUnitBuilder WithConstructorStrategy(IConstructorStrategy constructorStrategy)
        {
            _bigUnit = null;
            _constructorStrategy = constructorStrategy;
            return this;
        }

        public BigUnitBuilder WithDependencyProvider(IDependencyProvider dependencyProvider)
        {
            _bigUnit = null;
            _dependencyProvider = dependencyProvider;
            return this;
        }

        public BigUnit Build()
        {
            if (_bigUnit == null)
            {
                _bigUnit = new BigUnit(new TestInstanceStrategy(
                        _testScope,
                        _constructorStrategy),
                    new TestInstanceProvider(
                        _dependencyProvider,
                        new TypeCache()
                    )
                );
            }

            return _bigUnit;
        }
    }
}