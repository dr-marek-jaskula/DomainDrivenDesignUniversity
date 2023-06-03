# Tests.Unit Layer :heavy_check_mark:

This layer contains the unit tests. We distinguish:

1. LayerTests

	- **Domain** tests - test the domain logic
	- **Application** tests - test handlers and pipelines
	- **Infrastructure** tests - test tools
	- **Presentation** tests - test controllers

2. ArchitectureTests

	- Test dependencies, naming conventions and systems structure

3. UtilityTests

	- Test extension methods and similar utilities

Example of strongly type test data:
> SystemUnderTest/Domain/ValueObjects/ProductNameTests.cs

## Domain tests :anchor:

Domain tests are very fast. They should cover all cases, but be as simple as possible.

## Architecture tests :house:

For the architecture tests we use **NetArchTest.Rules** nuget package.
