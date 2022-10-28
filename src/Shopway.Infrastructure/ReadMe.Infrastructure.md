# Infrastructure Layer

This layer is responsible to implement the Contracts (Interfaces/Adapters) defined within the application layer to the Secondary Actors. Infrastructure Layer supports other layer by implementing the abstractions and integrations to 3rd-party library and systems.

Infrastructure layer contains most of your application’s dependencies on external resources such as file systems, web services, third party APIs, and so on. The implementation of services should be based on interfaces defined within the application layer.

In this project we store components connected to :

- Background jobs

- Identity Services
- File Storage Services
- Queue Storage Services
- Message Bus Services
- Payment Services
- Third-party Services
- Notifications
	- Email Service
	- Sms Service

## Bacground jobs by Quartz

Background jobs from Quartz NuGet Package have Scoped lifetime.
Therefore, we can inject scoped services.

In Shopway.App to configure Quartz background jobs we need Quartz.Extensions.Hosting NuGet Package. This integrates with background service in ASP.NET.

