# Domain Driven Design University

This is a project made for tutorial purpose - to clearly present the domain driven design concept.

## Solution structure

### .App

This layer idea is to register dependencies, set middlewares, provide options and run the program

### .Presentation

This layer idea is to provide the functionality for the end user, so controller and requests. 

Therefore it is like a "frontend" for the backed program.

### . Application

This layer idea is to provide handlers that are called by controllers. 

THerefore, we place here handlers pipelines, CQRS structure and mappings.

### .Infrastructure

This layer idea is to provide add additional, complex and external logic and tools that we are going to use.

Therefore, we place here services, validators, adapters, providers, policies, background jobs and so on.

### .Persistance

This layer idea is to provide the database specific structures, configurations, repositories, specifications.

### .Domain

This is the heart of the whole solution. We store here the entities, value objects, enumerations, domain events and general solution that our program will follow like results.