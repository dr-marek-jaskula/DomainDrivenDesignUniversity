# Tests.Unit Layer

This layer contains the unit tests. We distinguish:

1. LayerTests

	- **Domain** tests - test the domain logic
	- **Application** tests - test handlers and pipelines
	- **Infrastructure** tests - test tools
	- **Presentation** tests - test controllers

2. ArchitectureTests

	- Test dependencies, naming conventions and systems structure

3. UtilityTests

	- Test extension method and similar utilities

Example of strongly type test data:
> SystemUnderTest/Domain/ValueObjects/ProductNameTests.cs

## Domain tests

Domain tests are very fast. They should cover all cases, but be as simple as possible.

## Architecture tests

For the architecture tests we use **NetArchTest.Rules** nuget package.

## Naming Conventions

1. Tests should follow T1_T2_T3 convention where:
	- T1 is the name of the system functionality that is under test
	- T2 is the Expected result 
	- T3 is the test scenario

> GetById_ShouldSucceed_WhenCreateValidProduct

Note: Alternative naming conventions is to switch T2 with T3:

> GetById_WhenCreateValidProduct_ShouldSucceed

2. Mocks should end with "Mock" suffix"

> _productRepositoryMock

