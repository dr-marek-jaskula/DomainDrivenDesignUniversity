# Tests.Integration Layer

This layer contains the integrations tests. 

Here we use the proper approach of using the WebApplicationFactory.

Moreover, we use TestContianers library, to make all integrations test in sql database inside the container, so the additional database cleaning is not required (like in Shopway.Tests.Integration)