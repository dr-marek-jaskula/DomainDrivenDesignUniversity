# Domain Driven Design University

This is a project made to present the domain driven design concept.

By default the test database is used and it is cleaned after tests. 
User privileges should be added on the database level.

## Solution structure

### .App

This layer should register dependencies, set middlewares, provide options and run the program

### .Presentation

This layer should provide the functionality for the end user, so controllers and requests. 

Therefore, it is like a "frontend" for the backed program.

### .Application

This layer should provide handlers, that are called by controllers. 

Therefore, we place here handler pipelines, CQRS structure and mappings.

### .Infrastructure

This layer should provide additional tools that we are going to use.

Therefore, we place here services, validators, adapters, providers, policies, background jobs and so on.

### .Persistance

This layer should provide the database specific structures, configurations, repositories, specifications.

### .Domain

This is the heart of the whole solution. We store here entities, value objects, enumerations, domain events and other core structures.