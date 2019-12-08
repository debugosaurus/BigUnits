# BigUnits #

![GitHub Build Status](https://github.com/debugosaurus/BigUnits/workflows/.NET%20Core/badge.svg)

**BigUnits** is a library that aims to provide flexibility in how you scope "unit tests" within a solution - with the aim of supporting the development of tests that provide value beyond simply proving to others that your code works.

## Table of contents ##

## The problem ##

> The tests we write are not adding value, at least not anywhere near enough compared to the time spent writing and maintaining them

The biggest problem I've found with traditional unit tests is that they often do not relate back to the feature / business-case that spawned them; so if `ClassA` was written in servitude of `feature-x` then our tests would prove that `ClassA` works as written, but give us little to no direct confidence that `feature-x` is correct. 

Over time this gap between code functionality, and feature intent only grows wider, and eventually you end up with a test suite that does little to prove your system works. In the very worst cases you end up with tests that have little value other than proving that the compiler did what you expected it to do: `[Test] public void When_day_is_tuesday_then_false_is_returned()`

The reliance of tests at the low unit level can also make it harder to change the system later on. You cannot easily refactor or modify code because:
1) you have no confidence in how the code relates back to feature(s) - and so cannot easily reason about the changes you're making
2) you have no confidence that the changes you make won't negatively impact other features
3) you need to undertake an equal or greater level of effort to update corresponding tests

So the traditional way to mitigate the above issues is to use a mix of unit tests and integration / functional tests - using integration tests to ensure the system works at a feature level. Integration tests suits are not without issues either, with the biggest problem being the "live" nature of these tests. There are many, many different flavours of "integration" test (E2E, bottom-up, top-down...) but all typically require some external dependencies (database, API, hosting, configuration...) and as such these tests can quickly become brittle, flaky, difficult to setup, slow to run, hard to debug etc.

When an integration test suite becomes difficult to run you tend to find that people stop running them, and then they start ignoring flaky test results, then they start to ignore all test results, before long the only maintenance such suites get is to ensure the code compiles and doesn't block the build process.

## The solution ##

The solution, I think, is not that integration tests are bad, just that we need a way to better define and control their scope so we can model them more like traditional unit tests. This is what `BigUnits` attempts to do, it tries to apply **IOC** and **auto-mocking** from traditional unit tests to a wider scope - effectively allowing you to write "unit" tests that span beyond the traditional class scope.