# BigUnits

![GitHub Build Status](https://github.com/debugosaurus/BigUnits/workflows/.NET%20Core/badge.svg)

**BigUnits** is a library that aims to provide flexibility in how you scope "unit tests" within a solution - with the aim of supporting the development of [sociable](https://martinfowler.com/articles/practical-test-pyramid.html#SociableAndSolitary) units tests.

The core aim of **BigUnits** is to support developers in choosing the right level of testing for the code they write - rather than defaulting either one of two extremes of solitary unit tests, or end-to-end integration tests. It achieves this by providing simple, extensible, test fixture management that enables you to control the scope (what gets mocked vs what should be real) of your tests - so you can test at the level that make the most sense for the type of components you're developing.

The way **BigUnits** works is to provide a simple [auto-mocking container](https://blog.ploeh.dk/2013/03/11/auto-mocking-container/) that provides real vs mocked test doubles according to the scope you set for a test. A traditional solitary unit test would only provide a real instances of the class being tested, and every other dependency would be mocked; while a sociable test scoped to a namespace would mock everything outside of the nominated namespace, and provide real instances for anything within the namespace.

## Features

**BigUnits** is in a very early stage of development, so there are **BIG** gaps in what it can do, but at a glance:

* Support for the following test scopes:
    * class scope (ie: solitary / strict unit test)
    * namespace scope (ie: use real instance of classes under a shared namespace and mock everything else)
* Auto mock constructor dependencies using **Moq**
* Provide concrete test instances for classes within a defined scope
* Override scoping and mocking behaviour by explicitly nominating instances to use

## Benefits

This isn't intended to be a one-size fits all solution, and I'd still encourage people to write integration tests and solitary unit tests where it makes sense, however I believe there are benefits to exploring sociable unit tests (and hopefully using this framework).

Anecdotally **BigUnits** is used to test **BigUnits** (it uses itself to test itself), and I've found it really opens up the ability to do true [TDD](https://en.wikipedia.org/wiki/Test-driven_development) in a way I've never really been able to achieve at the class level. Too often I'd find myself having to scope out / fully design class structures before starting tests or otherwise end up continually refactoring both tests and design. Doing TDD at a higher level provides the freedom to concentrate on the feature you're intending to deliver rather than the specifics of the implementation - which in turn means you can do rapid [MVP](https://en.wikipedia.org/wiki/Minimum_viable_product) style delivery. 

The other cool thing is you _can_ refactor / recompose without having to touch tests. As an example I'm pretty unhappy with much of the internal composition of this library, but I'm happier now than I was previously when things were mostly contained in one single god class. Using the framework I was able to split up and rewire my implementation and still be able to run the same test suite over the top - so there was no need to touch the test suite at all. Being able to jump straight into "brave" refactoring work is something we're seldom able to do, and it's pretty cool to have a test suite that encourages this.

You can also cut way back on the number of unnecessary interfaces used in your codebase - a common complaint against SOLID code. 

On the flip side I am finding that the above MVP approach can lead to pretty shitty code, but again so long as that code works in the context it was defined for I'm not too worried.

# Why?

> The tests we write are not adding value, at least not anywhere near enough compared to the time spent writing and maintaining them

I'd bet that the majority of tests written by developers falls into the solitary unit test category; and for good reason since unit tests are normally simple to write, quick to execute, and provide a good degree of confidence that the code you write works. They're great for testing low level, reusable components that need to work towards an established specification.

For testing the validity of a "business" feature, however, they're pretty shit.

The biggest problem I've found, however, with traditional unit tests is that they often do not relate back to the feature / business-case that spawned them; so if `ClassA` was written in servitude of `feature-x` then our tests would prove that `ClassA` works as written, but give us little to no direct confidence that `feature-x` is correct. 

Over time this gap between code functionality, and feature intent only grows wider, and eventually you end up with a test suite that does little to prove your system works. In the very worst cases you end up with tests that have little value other than proving that the compiler did what you expected it to do.

The reliance of tests at the low unit level can also make it harder to change the system later on, and when you do attempt refactoring works you end up having to through away the entirety of your suite and rewriting the tests from scratch - so the thing that proves your system works cannot be relied upon to prove it works should the implementation change.

So the traditional way to mitigate the above issues is to use a mix of unit tests and integration / functional tests - using integration tests to ensure the system works at a feature level. Integration tests suits are not without issues either, with the biggest problem being the "live" nature of these tests. There are many, many different flavours of "integration" test (E2E, bottom-up, top-down...) but all typically require some external dependencies (database, API, hosting, configuration...) and as such these tests can quickly become brittle, flaky, difficult to setup, slow to run, hard to debug etc.

When an integration test suite becomes difficult to run you tend to find that people stop running them, and then they start ignoring flaky test results, then they start to ignore all test results, before long the only maintenance such suites get is to ensure the code compiles and doesn't block the build process.

## A (strawman) scenario

Imagine you've been tasked with writing a feature that displays a popup on a website that informs users about cookie usage. A key requirement is that the text of the popup should be configurable without re-deploying the site. Given timeframes / motivation you've decided that the most straightforward thing to do is to store the text in a `.txt` file on a shared drive that can be accessed by your API, and simply write an endpoint that reads and returns this text each time it's asked for.

You build the feature and write unit tests to verify that your code can read and return text from a mocked network share as expected. All is good until a tester notices that when they exercise the feature in a test environment they see some weird symbols in the popup text. After investigation it turns out that the tester and you use different text editors with different defaults, and one defaults to UTF-8 with a BOM, the other to UTF-8 without a BOM. You code a quick fix and extend your tests to cope for both UTF-8 variants.

This time it passes the QA stage and is deployed to production. Hooray!

A short time later you're asked to implement rich formatting for this popup. At this point it makes sense to integrate with your company's CMS rather than read from a file, so you re-implement the feature to be driven from this external system instead. During re-implementation you notice that the majority of the tests you'd written deal with implementation details you're now throwing away and replacing; the UTF-8 BOM / no-BOM tests have no relevance to the reimplemented feature and so have to be scrapped.

So what value did your unit tests provide in this instance? Not very much. They tested that the code you wrote, did the task that you wrote it to do - depending on how [SOLID](https://en.wikipedia.org/wiki/SOLID) your code is you possibly just ended up testing that the compiler works more than anything. In terms of the feature that sponsored the code's creation your tests achieved fuck all, in fact they may have even slowed you down in the long run.

This, trite, example is intended to show that unit tests are not always a good choice for every situation. In fact I'd say that unless you're explicitly setting out to create reusable / shared components, or things like rules that are required to follow a strict low-level specification, then solitary unit tests are probably a poor choice - especially when the implementation details may still be in flux. Integration tests would be a far better choice in this instance, though (as mentioned above) these can have a poor time-to-feedback.

## Sociable unit tests

Sociable unit tests blur the boundaries between the more traditional solitary unit tests, and integration tests, as they operate at an expanded scope - instantiating two or more "real" classes per test. Using an expanded scope means we can test features / sub-features more cohesively; and crucially we can related these tests directly back to the functionality that sponsored them.

With the above scenario we could engineer sociable tests that combine the various components that comprise the popup text API. We can then define higher level expectations that outline the operational requirements for the feature (rather than for the implementation).

This won't magically mean we can change the implementation and won't have to update the tests, but it should mean that the test outcomes / intent should remain constant - giving you greater confidence that the feature continues to work as expected.

# Usage

The library is very opinionated at the moment, but over time I hope to iron out assumptions and limitations. 

## Example base test class

The intent is for consumers to declare abstract test bases that use the `BigUnit` class to driven test functionality - the rationale being that this allows you to wire up your tests in the way that most makes sense to your needs, along with enabling you to mix and match various frameworks.

As an example (see: [UnitTest.cs](./tests/Debugosaurus.BigUnits.Tests/IntegrationTest.cs)) - where, for my purposes, I've decided that an integration test is scoped to the namespace (and all child namespaces) of a nominated class.

The library will at somepoint provide some boilerplate implementations for these base classes (once the library is more stable).

```CSharp
public abstract class IntegrationTest<T> where T : class
{
    private BigUnitBuilder _bigUnitBuilder;

    protected IntegrationTest()
    {
        _bigUnitBuilder = new BigUnitBuilder()
            .WithTestScope(TestScopes.Namespace<T>())
            .WithDependencyProvider(new MoqDependencyProvider());
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
* `<T>` - a class within the namespace that is used to define the scope
* `TestInstance` - Gets a instance that will be used in the test - often called the **SUT**
* `GetDependency` - retrieves the dependency that was / will be used when creating the `TestInstance`s
* `SetDependency` - sets the dependency that will be used when creating the `TestInstance`s

## Dependency provider libraries

**BigUnits** is extendible and aims to support a variety of test tools. At present it only supports **Moq** via the **Debugosaurus.BigUnits.Moq** library. 

## Example test

A poor example, but the idea is that the feature below orchestrates the sanitising and formatting of the popup message.

```CSharp

public class CookiePopupFeature
{
    private ICurrentUser _user;
    private ICookieMessageProvider _cookieMessageProvider;

    private CookieMessageFormatter _cookieMessageFormatter;
    private CookieMessageSanitiser _cookieMessageSanitiser;
    private CookieMessageHtmlWrapper _cookieMessageHtmlWrapper;

    public CookiePopup GetCookiePopup()
    {
        if(_user.HasAcceptedCookies)
        {
            return CookiePopup.Empty;
        }

        var messageTemplate = _cookieMessageProvider.GetTemplate();
        var message = _cookieMessageFormatter.Format(_user.IsLoggedIn ? _user.DisplayName : "you");
        message = _cookieMessageSanitiser.Sanitise(message);
        message = _cookieMessageHtmlWrapper.WrapMessage(message);

        return new CookiePopup(
            true,
            messageTemplate,
            message
        );
    }
}

```

```CSharp
public class CookiePopupTests : IntegrationTest<CookiePopupFeature>
{
    private const string TestCookieText = "Hi {USERNAME}, our site uses COOKIES!! <script>alert('unsafe')</script>";
    private const string ExpectedFormattedCookieText = "<div class=\"cookiePopup\">Hi you, our site uses COOKIES!!</div>";

    [Fact]
    public void ReturnsFormattedCookieTextWhenUserHasNotAcceptedCookies()
    {     
        GivenTheUserHasNotAcceptedCookies();
        GivenTheMessageTemplateIs(TestCookieText);
        
        var result = TestInstance.GetCookiePopup();

        result.ShouldBeEquivalentTo(new {
            Display = true,
            RawText = TestCookieText,
            Text = ExpectedFormattedCookieText
        });
    }

    private void GivenTheUserHasNotAcceptedCookies()
    {
        GetDependency<ICurrentUser>()
            .Setup(x => x.HasAcceptedCookies)
            .Returns(false);
    }

    private void GivenTheMessageTemplateIs(string messageTemplate)
    {
        GetDependency<ICookieMessageProvider>()
            .Setup(x => x.GetTemplate())
            .Returns(messageTemplate)
    }
}

```