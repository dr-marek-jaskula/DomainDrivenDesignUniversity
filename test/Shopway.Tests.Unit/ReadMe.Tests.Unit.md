# Tests.Unit Layer

This layer contains the unit tests. We distinguish:

1. LayerTests
	a. **Domain** tests - test the domain logic
	b. **Application** tests - test handlers and pipelines
	c. **Infrastructure** tests - test tools
	d. **Presentation** tests - test controllers

2. ArchitectureTests
	a. Test dependencies, naming conventions and systems structure

3. UtilityTests
	a. Test extension method and similar utilities

Example of strongly type test data:
> SystemUnderTest/Domain/ValueObjects/ProductNameTests.cs

## Domain tests

Domain tests are very fast. They should cover all cases, but be as simple as possible.

## Architecture tests

For the architecture tests we use **NetArchTest.Rules** nuget package.