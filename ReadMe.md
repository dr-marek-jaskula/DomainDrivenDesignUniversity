# Domain Driven Design University :school:

This project presents the domain driven design concepts.

Domain and Application projects are structured using vertical slices/screaming architecture/by aggregates.
Nevertheless, for Infrastructure, Persistence, Presentation projects, I have decided to create a folder structure based on respective types.
From my perspective it is a preferred approach, because these projects do not have as many aggregate-related structures.
Thus organizing them in screaming architecture would result in less readable and less maintainable solution.

## Give a Star! :star:

If you like or are using this project to learn about Domain Driven Design, please give it a star. Thanks!

## Solution structure :mag:

The solution in designed using the Clean Architecture in the following form. 

![VeryCleanArchitecture-removebg](https://github.com/dr-marek-jaskula/DomainDrivenDesignUniversity/assets/76631630/dcb809f9-f0cf-40f5-bba2-a46dfa16831a)

I prefer to present it like this instead of using other Clean Architecture images, because in my opinion this is more readable. The runner layer is required so that the Presentation, Infrastructure and Persistence layers can be truly separated.

### .App :car:

This layer should register dependencies and run the program. In terms of presented architecture schema it is a runner.

### .Presentation :door: 

This layer should provide endpoints for the end user. 

Therefore, it is like a "frontend" for the backed program.

### .Application :computer:

This layer should provide handlers that orchestrate domain objects to obtain business features. 

Therefore, we place here middlewares, CQRS structure and mappings.

### .Infrastructure :factory:

This layer should provide additional tools (not database specific) that we are going to use.

Therefore, we place here services, options, validators, adapters, providers, policies and so on.

### .Persistence :books:

This layer should provide database specific structures, configurations, repositories, specifications, background jobs.

### .Domain :anchor:

This is the heart of the whole solution. We store here entities, value objects, enumerations, domain events and other core structures.

### .Tests.Unit :heavy_check_mark:

Project to present structure of unit tests.

It contains also domain tests, architecture tests and utility tests.

### .Tests.Integration :heavy_check_mark:

This layer contains integrations tests. 

Here we do not use the WebApplicationFactory approach, but we create DI container in more manual way.

Moreover, tests are done on the test database, but the test cleaning is required (see TestDataGeneratorBase.CleanDatabaseFromTestData).

Note: use these tests without containerized application and for the database that is locally installed.

### .Tests.Integration.Container :heavy_check_mark:

This layer contains integrations tests. 

Here we use the proper approach of using the WebApplicationFactory.

Moreover, we use TestContainers library, to make all integrations tests in sql database inside the container, so the additional database cleaning is not required (like in Shopway.Tests.Integration).

### .Tests.Performance :heavy_check_mark:

This layer contains performance tests.

We use NBomber as a performance test framework, because we can use it with xunit.

Note: use these tests without containerized application and for the database that is locally installed.

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

### Run Application with Local Database :arrow_forward:

1. Select **Shopway.App** as startup project. 
2. Create databases called **DDDUniveristy** and **DDDUniversityTest**.
3. Run the application

### Run Containerized Application with Containerized Database :whale:

Note: Portainer is used to investigate containers if required. 

1. Select **docker-compose** as startup project.
2. Run the application

Alternatively: 
1. Open console at the solution level ```..\DomainDrivenDesignUniversity>```
2. Run in detached mode:
```console
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

To stop containers use:
```console
docker compose -f docker-compose.yml -f docker-compose.override.yml down
```

If image rebuild is required:
```console
docker compose -f .\docker-compose.yml -f .\docker-compose.override.yml up --build
```

### Postman Collection :construction:

To get all postman endpoints configured for local environment and docker containers:
1. Go to ```docs``` folder.
2. Import collection ```DomainDrivenDesignUniversity.postman_collection.json```
3. Import workspace globals ```workspace.postman_globals```
4. Import local postman environment ```LOCAL.postman_environment```
5. Import docker postman environment ```DOCKER.postman_environment```

All collection and global variables are set after requests. For instance, **Login** endpoint automatically
sets a bearer token. Then, this token is used in the authentication process.


### GitHub Workflows :camel:

Currently there are two workflows setup for this repository:
1. Continuous Integration
	- This workflow setups and builds the application. Then, it runs unit tests and publishes test reports.
2. Conventional Pull Request Validation
	- This workflow validates if the pull request name matches the conventional commit rules. If validation succeeds, it adds a meaningful label to the pull request.
