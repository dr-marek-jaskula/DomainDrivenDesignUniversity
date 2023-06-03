# Domain Driven Design University

This project presents the domain driven design concepts.

I have decided to organize the domain project using folders for respective types. For smaller project (without many entities) it is a preferred approach. Moreover, this structure is easier to follow when studying Domain Driven Design. Nevertheless, for larger projects, the preferred approach is to organize the domain by aggregates, so one folder should contain all files connected to respective aggregate, more or less like vertical slices in the application layer.

## Solution structure

### .App

This layer should register dependencies and run the program

### .Presentation

This layer should provide the functionality for the end user, so controllers and requests. 

Therefore, it is like a "frontend" for the backed program.

### .Application

This layer should provide handlers, that are called by controllers. 

Therefore, we place here handler pipelines, CQRS structure and mappings.

### .Infrastructure

This layer should provide additional tools that we are going to use.

Therefore, we place here services, options, validators, adapters, providers, policies, background jobs and so on.

### .Persistance

This layer should provide the database specific structures, configurations, repositories, specifications.

### .Domain

This is the heart of the whole solution. We store here entities, value objects, enumerations, domain events and other core structures.

### .Tests.Unit

Project to present structure of unit tests.

It contains also domain tests, architecture tests and utility tests.

### .Tests.Integration

This layer contains the integrations tests. 

Here we do not use the WebApplicationFactory approach, but we create DI container in more manual way.

Moreover, tests are done on the test database, but the test cleaning is required (see TestDataGeneratorBase.CleanDatabaseFromTestData)

### .Tests.Integration.Container

This layer contains the integrations tests. 

Here we use the proper approach of using the WebApplicationFactory.

Moreover, we use TestContainers library, to make all integrations test in sql database inside the container, so the additional database cleaning is not required (like in Shopway.Tests.Integration)

### .Tests.Performance

This layer container the performance tests.

We use NBomber as a performance test framework, because we can use it with xunit.

### .Tests Naming Conventions

1. Tests should follow T1_T2_T3 convention where:
	- T1 is the name of the system functionality that is under test
	- T2 is the expected result 
	- T3 is the test scenario

> GetById_ShouldSucceed_WhenCreateValidProduct

Note: Alternative naming convention: switch T2 with T3:

> GetById_WhenCreateValidProduct_ShouldSucceed

2. Mocks should end with "Mock" suffix"

> _productRepositoryMock

### Postman Collection

Use the postman collection to get all endpoints for postman. 

Replace variables current values like "validProductGuid" or "validReviewGuid" to your custom ones.
