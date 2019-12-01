namespace Debugosaurus.BigUnits.Framework
{
    public class BigUnitBuilder
    {
        private ITestScope testScope;

        private IConstructorStrategy constructorStrategy;

        private IDependencyProvider dependencyProvider;

        private BigUnit bigUnit;

        public BigUnitBuilder()
        {
            testScope = new GlobalTestScope();
            constructorStrategy = new GreedyConstructorStrategy();
        }

        public BigUnitBuilder WithTestScope(ITestScope testScope)
        {
            bigUnit = null;
            this.testScope = testScope;
            return this;
        }

        public BigUnitBuilder WithConstructorStrategy(IConstructorStrategy constructorStrategy)
        {
            bigUnit = null;
            this.constructorStrategy = constructorStrategy;
            return this;
        }

        public BigUnitBuilder WithDependencyProvider(IDependencyProvider dependencyProvider)
        {
            bigUnit = null;
            this.dependencyProvider = dependencyProvider;
            return this;
        }

        public BigUnit Build()
        {
            if(bigUnit == null)
            {
                bigUnit = new BigUnit(
                    testScope,
                    new TestInstanceProvider(
                        testScope,
                        constructorStrategy,
                        dependencyProvider,
                        new TypeCache()
                    )
                );
            }
            return bigUnit;
        }
    }
}