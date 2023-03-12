# Domain Driven Design University

This project presents the domain driven design concepts.

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

Project to present structure of unit tests

### .Tests.Integration

This layer contains the integrations tests. 

Here we do not use the WebApplicationFactory approach, but we create DI container in more manual way.

Moreover, tests are done on the test database, but the test cleaning is required (see TestDataGeneratorBase.CleanDatabaseFromTestData)

### .Tests.Integration.Container

This layer contains the integrations tests. 

Here we use the proper approach of using the WebApplicationFactory.

Moreover, we use TestContianers library, to make all integrations test in sql database inside the container, so the additional database cleaning is not required (like in Shopway.Tests.Integration)