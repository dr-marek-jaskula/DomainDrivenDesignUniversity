# Tests.Unit Layer

This layer contains the unit tests. We distinguish:

I. LayerTests
	1. **Domain** tests - test the domain logic
	2. **Application** tests - test handlers and pipelines
	3. **Infrastructure** tests - test tools
	4. **Presentation** tests - test controllers

II. ArchitectureTests
	1. Test dependencies, naming conventions and systems structure

III. UtilityTests
	1. Test extension method and similar utilities

Example of strongly type test data:
> SystemUnderTest/Domain/ValueObjects/ProductNameTests.cs

## Domain tests

Domain tests are very fast. They should cover all cases, but be as simple as possible.

## Architecture tests

For the architecture tests we use **NetArchTest.Rules** nuget package.