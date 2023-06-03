# Domain Driven Design University :school:

This project presents the domain driven design concepts.

I have decided to organize the domain project using folders for respective types. For smaller project (without many entities) it is the preferred approach. Moreover, this structure is easier to follow when studying Domain Driven Design. Nevertheless, for larger projects, the preferred approach is to organize the domain by aggregates, so one folder should contain all files connected to respective aggregate, more or less like vertical slices in the application layer.

## Give a Star! :star:

If you like or are using this project to learn about Domain Driven Design, please give it a star. Thanks!

## Solution structure :mag:

### .App :car:

This layer should register dependencies and run the program.

### .Presentation :door: 

This layer should provide endpoints for the end user. 

Therefore, it is like a "frontend" for the backed program.

### .Application :computer:

This layer should provide handlers, that are called by controllers. 

Therefore, we place here handler pipelines, CQRS structure and mappings.

### .Infrastructure :factory:

This layer should provide additional tools that we are going to use.

Therefore, we place here services, options, validators, adapters, providers, policies, background jobs and so on.

### .Persistance :books:

This layer should provide database specific structures, configurations, repositories, specifications.

### .Domain :anchor:

This is the heart of the whole solution. We store here entities, value objects, enumerations, domain events and other core structures.

### .Tests.Unit :heavy_check_mark:

Project to present structure of unit tests.

It contains also domain tests, architecture tests and utility tests.

### .Tests.Integration :heavy_check_mark:

This layer contains integrations tests. 

Here we do not use the WebApplicationFactory approach, but we create DI container in more manual way.

Moreover, tests are done on the test database, but the test cleaning is required (see TestDataGeneratorBase.CleanDatabaseFromTestData).

### .Tests.Integration.Container :heavy_check_mark:

This layer contains integrations tests. 

Here we use the proper approach of using the WebApplicationFactory.

Moreover, we use TestContainers library, to make all integrations test in sql database inside the container, so the additional database cleaning is not required (like in Shopway.Tests.Integration).

### .Tests.Performance :heavy_check_mark:

This layer contains performance tests.

We use NBomber as a performance test framework, because we can use it with xunit.

### .Tests Naming Conventions :scroll:

1. Tests should follow T1_T2_T3 convention where:
	- T1 is the name of the system functionality that is under test
	- T2 is the expected result 
	- T3 is the test scenario

> GetById_ShouldReturnProduct_WhenProductExists

Note: Alternative naming convention: switch T2 with T3:

> GetById_WhenProductExists_ShouldReturnProduct

2. Mocks should end with "Mock" suffix:

> _productRepositoryMock

### Postman Collection :construction:

Use the postman collection to get all endpoints for postman. 

Replace variables current values like "validProductGuid" or "validReviewGuid" to your custom ones.
