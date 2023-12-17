# Tests.Integration Layer :heavy_check_mark:

This layer contains the integrations tests. 

Here we do not use the WebApplicationFactory approach, but we create DI container in more manual way.

Moreover, tests are done on the test database, but the test cleaning is required (see TestDataGeneratorBase.CleanDatabaseFromTestData)

Use these tests without containerized application and for the database that is locally installed.

Test user is registered with all roles in ControllerTestBase
```csharp
var roles = typeof(User)
    .GetField("_roles", BindingFlags.NonPublic | BindingFlags.Instance)!
    .GetValue(user) as List<Role>;

roles!.AddRange(Role.List);
```