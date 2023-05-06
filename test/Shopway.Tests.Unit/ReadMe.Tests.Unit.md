# Tests.Unit Layer

This layer contains the unit tests. 

We distinguish:

1. **Domain** tests - test the domain logic
2. **Application** tests - test handlers and pipelines
3. **Infrastructure** tests - test tools
4. **Presentation** tests - test controllers
5. **Architecture** tests - we use **ArchUnitNET.xUnit** to test dependencies, naming conventions and more
6. **Utilities** tests - we should test extension method and similar utilities

For example of strongly type test data see:
> SystemUnderTest/Domain/ValueObjects/ProductNameTests.cs

## Domain tests

Domain tests are very fast. They should cover all cases, but be as simple as possible.

## Architecture tests

For the architecture tests we use **NetArchTest.Rules** nuget package.