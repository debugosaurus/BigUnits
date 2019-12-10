# BigUnits

![GitHub Build Status](https://github.com/debugosaurus/BigUnits/workflows/.NET%20Core/badge.svg)

**BigUnits** is a library that aims to provide flexibility in how you scope "unit tests" within a solution - with the aim of supporting the development of [sociable](https://martinfowler.com/articles/practical-test-pyramid.html#SociableAndSolitary) units tests that provide value beyond simply proving to others that your code works.

Pretty much everywhere I've worked has had their own unit test base classes that implement some form of [auto-mocking container](https://blog.ploeh.dk/2013/03/11/auto-mocking-container/) in order to simplify the creation and maintenance of unit tests (where they haven't I've been quick to introduce it). Without exception this pattern has been constrained to solitary / strict unit tests only. **BigUnits** aims to provide the benefits of auto-mocking to larger scoped tests - allowing you to use real test instances of classes within a namespace, for example, while mocking everything outside of it.

# Why?

> The tests we write are not adding value, at least not anywhere near enough compared to the time spent writing and maintaining them

The biggest problem I've found with traditional unit tests is that they often do not relate back to the feature / business-case that spawned them; so if `ClassA` was written in servitude of `feature-x` then our tests would prove that `ClassA` works as written, but give us little to no direct confidence that `feature-x` is correct. 

Over time this gap between code functionality, and feature intent only grows wider, and eventually you end up with a test suite that does little to prove your system works. In the very worst cases you end up with tests that have little value other than proving that the compiler did what you expected it to do: `[Test] public void When_day_is_tuesday_then_false_is_returned()`

The reliance of tests at the low unit level can also make it harder to change the system later on. You cannot easily refactor or modify code because:
1) you have no confidence in how the code relates back to feature(s) - and so cannot easily reason about the changes you're making
2) you have no confidence that the changes you make won't negatively impact other features
3) you need to undertake an equal or greater level of effort to update corresponding tests

So the traditional way to mitigate the above issues is to use a mix of unit tests and integration / functional tests - using integration tests to ensure the system works at a feature level. Integration tests suits are not without issues either, with the biggest problem being the "live" nature of these tests. There are many, many different flavours of "integration" test (E2E, bottom-up, top-down...) but all typically require some external dependencies (database, API, hosting, configuration...) and as such these tests can quickly become brittle, flaky, difficult to setup, slow to run, hard to debug etc.

When an integration test suite becomes difficult to run you tend to find that people stop running them, and then they start ignoring flaky test results, then they start to ignore all test results, before long the only maintenance such suites get is to ensure the code compiles and doesn't block the build process.

# Usage

The library is very opinionated at the moment, but over time I hope to iron out assumptions and limitations. The intent is for consumers to declare abstract test bases that use the `BigUnit` class to driven test functionality. As an example (see: [UnitTest.cs](./tests/Debugosaurus.BigUnits.Tests/UnitTest.cs))

```CSharp
public abstract class UnitTest<T> where T : class
{
    private BigUnitBuilder _bigUnitBuilder;

    protected UnitTest() 
    {
        _bigUnitBuilder = new BigUnitBuilder()
            .WithTestScope(TestScopes.Class<T>())
            .WithDependencyProvider(new NotImplementedDependencyProvider());
    }

    private BigUnit BigUnit => _bigUnitBuilder.Build();

    protected T TestInstance => BigUnit.GetTestInstance<T>();

    protected void SetDependency<TDependency>(TDependency dependency)
    {
        BigUnit.SetDependency(dependency);
    }

    protected TDependency GetDependency<TDependency>()
    {
        return BigUnit.GetDependency<TDependency>();
    }    
}
```
* `TestInstance` - Gets the instance that will be tested - often called the **SUT**
* `GetDependency` - retrieves the dependency that was / will be used when creating the `TestInstance`
* `SetDependency` - sets the dependency that will be used when creating the `TestInstance`

# Features

**BigUnits** is in a very early stage of development, so there are big gaps in what it can do, but at a glance:

* Auto mock constructor dependencies using *Moq*
* Support for the following test scopes:
    * class scope (ie: solitary / strict unit test)
    * namespace scope (ie: use real instance of classes within a shared namespace and mock everything else)
* Provide concrete test instances for classes within a defined scope
* Override scoping and mocking behaviour by explicitly nominating instances to use