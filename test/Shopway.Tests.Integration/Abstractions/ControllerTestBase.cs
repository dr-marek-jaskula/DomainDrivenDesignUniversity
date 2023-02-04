using Microsoft.EntityFrameworkCore;
using RestSharp;
using RestSharp.Authenticators;
using Shopway.Application.CQRS.Users.Commands.CreateUser;
using Shopway.Application.CQRS.Users.Commands.LogUser;
using Shopway.Domain.Entities;
using Shopway.Tests.Integration.Persistance;
using Shopway.Tests.Integration.Utilities;
using System.Data;
using Shopway.Persistence.Constants;
using Shopway.Domain.EntityIds;
using Gatherly.Presentation.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Tests.Integration.Configurations;
using Shopway.Tests.Integration.Constants;
using static RestSharp.Method;
using static Shopway.Tests.Integration.Constants.IntegrationTestsConstants;
using Shopway.Domain.Enumerations;

namespace Shopway.Tests.Integration.Abstractions;

public abstract class ControllerTestsBase : IDisposable
{
    private readonly string ShopwayApiUrl;
    private readonly RestClient _userClient; 
    protected readonly string _controllerUri;
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsUrlOptions integrationTestsUrlOptions;

    public ControllerTestsBase(DependencyInjectionContainerTestFixture containerTestFixture)
	{
        Scope = containerTestFixture
            .ServiceProvider
            .CreateScope();

        integrationTestsUrlOptions = (IntegrationTestsUrlOptions)containerTestFixture
            .ServiceProvider
            .GetRequiredService(typeof(IntegrationTestsUrlOptions));

        ShopwayApiUrl = integrationTestsUrlOptions.ShopwayApiUrl!;
        _controllerUri = GetType().Name[..^ControllerTests.Length];
        _userClient = new($"{ShopwayApiUrl}{nameof(User)}");
    }

    protected async Task<RestClient> RestClient(string controllerUri, DatabaseFixture databaseFixture)
	{
        var client = new RestClient($"{ShopwayApiUrl}{controllerUri}");

        await EnsureThatTheTestUserIsRegistered(databaseFixture);
        var token = await LogTestUser();

        client.UseAuthenticator(new JwtAuthenticator(token));

        return client;
    }

	protected static RestRequest GetRequest(string endpointUri)
	{
		return new RestRequest(endpointUri, Get);
    }

    protected static RestRequest PostRequest(string endpointUri, object body)
    {
        var request = new RestRequest(endpointUri, Post);

        return request
            .AddJson(body);
    }

    protected static RestRequest PatchRequest(string endpointUri, object body)
    {
        var request = new RestRequest(endpointUri, Patch);

        return request
            .AddJson(body);
    }

    protected static RestRequest DeleteRequest(string endpointUri)
    {
        return new RestRequest(endpointUri, Delete);
    }

    /// <summary>
    /// Ensure that the test user is registered
    /// </summary>
    /// <param name="databaseFixture"></param>
    /// <returns>Jwt token</returns>
    private async Task EnsureThatTheTestUserIsRegistered(DatabaseFixture databaseFixture)
    {
        var userAlreadyExists = await databaseFixture
            .Context
            .Set<User>()
            .Where(x => x.Username.Value == TestUser.Username)
            .AnyAsync();

        if (userAlreadyExists is true)
        {
            return;
        }

        var registerCommand = new CreateUserCommand(TestUser.Username, TestUser.Email, TestUser.Password, TestUser.Password);

        var registerRequest = PostRequest(nameof(UserController.Register), registerCommand);

        await _userClient.PostAsync(registerRequest);

        var user = databaseFixture
            .Context
            .Set<User>()
            .First();

        //Give all roles to the user
        foreach (var role in Role.Ids)
        {
            await databaseFixture.Context.Database.ExecuteSqlRawAsync(@$"
            INSERT INTO {SchemaNames.Master}.{TableNames.RoleUser} (RoleId, {nameof(UserId)})
            VALUES ({role}, '{user.Id.Value}');     
            ");
        }

        await databaseFixture.Context.SaveChangesAsync();
    }

    /// <summary>
    /// Register and log the test user
    /// </summary>
    /// <param name="databaseFixture"></param>
    /// <returns>Jwt token</returns>
    private async Task<string> LogTestUser()
    {
        var logCommand = new LogUserCommand(TestUser.Email, TestUser.Password);

        var loginRequest = PostRequest(nameof(UserController.Login), logCommand);

        var logResponse = await _userClient.PostAsync(loginRequest);

        var token = logResponse.Deserialize<LogUserResponse>();

        return token!.Token;
    }

    public virtual void Dispose()
    {
        Scope.Dispose();
    }
}