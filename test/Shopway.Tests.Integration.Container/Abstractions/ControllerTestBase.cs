using Microsoft.EntityFrameworkCore;
using RestSharp;
using RestSharp.Authenticators;
using Shopway.Application.CQRS.Users.Commands.RegisterUser;
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
using Shopway.Domain.Enumerations;
using static RestSharp.Method;

using static Shopway.Tests.Integration.Constants.IntegrationTestsConstants;

namespace Shopway.Tests.Integration.Abstractions;

public abstract class ControllerTestsBase
{
    private readonly RestClient _userClient;
    protected HttpClient httpClient;
    protected readonly string shopwayApiUrl;
    protected readonly string controllerUrl;
    protected readonly ApiKeyTestOptions apiKeys;
    protected readonly DatabaseFixture fixture;

    public ControllerTestsBase(ShopwayApiFactory apiFactory)
    {
        apiKeys = apiFactory.Services.GetRequiredService<ApiKeyTestOptions>();
        shopwayApiUrl = apiFactory.Services.GetRequiredService<IntegrationTestsUrlOptions>().ShopwayApiUrl!;
        controllerUrl = GetType().Name[..^ControllerTests.Length];

        httpClient = apiFactory.CreateClient();
        var userUri = new Uri($"{shopwayApiUrl}{nameof(UsersController)[..^Controller.Length]}");
        _userClient = new(httpClient, new RestClientOptions(userUri));
        fixture = new DatabaseFixture(apiFactory.ContainerConnectionString);
    }

    /// <summary>
    /// Create the rest client with api url appended by given controller url and ensure that the test user for this client has all privileges
    /// </summary>
    /// <param name="httpClient">Controller url</param>
    /// <param name="databaseFixture">Database fixture</param>
    /// <returns></returns>
    protected async Task<RestClient> RestClient(HttpClient httpClient, RestClientOptions restClientOptions, DatabaseFixture databaseFixture)
    {
        await EnsureThatTheTestUserIsRegistered(databaseFixture);
        var token = await LogTestUser();

        restClientOptions.Authenticator = new JwtAuthenticator(token);
        return new RestClient(httpClient, restClientOptions);
    }

    /// <summary>
    /// Get Request 
    /// </summary>
    /// <param name="endpointUri">Endpoint</param>
    /// <returns></returns>
	protected static RestRequest GetRequest(string endpointUri)
	{
		return new RestRequest(endpointUri, Get);
    }

    /// <summary>
    /// Post Request
    /// </summary>
    /// <param name="endpointUri">Endpoint uri</param>
    /// <param name="body">Request body</param>
    /// <returns></returns>
    protected static RestRequest PostRequest(string endpointUri, object body)
    {
        var request = new RestRequest(endpointUri, Post);

        return request
            .AddJson(body);
    }

    /// <summary>
    /// Patch Request
    /// </summary>
    /// <param name="endpointUri">Endpoint uri</param>
    /// <param name="body">Request body</param>
    /// <returns></returns>
    protected static RestRequest PatchRequest(string endpointUri, object body)
    {
        var request = new RestRequest(endpointUri, Patch);

        return request
            .AddJson(body);
    }

    /// <summary>
    /// Delete Request
    /// </summary>
    /// <param name="endpointUri">Endpoint uri</param>
    /// <returns></returns>
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
            .Where(x => (string)(object)x.Username == TestUser.Username)
            .AnyAsync();

        if (userAlreadyExists is true)
        {
            return;
        }

        var registerCommand = new RegisterUserCommand(TestUser.Username, TestUser.Email, TestUser.Password, TestUser.Password);

        var registerRequest = PostRequest(nameof(UsersController.Register), registerCommand);

        await _userClient.PostAsync(registerRequest);

        var user = await databaseFixture
            .Context
            .Set<User>()
            .Where(user => (string)(object)user.Username == TestUser.Username)
            .FirstAsync();

        //Give all roles to the test user
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

        var loginRequest = PostRequest(nameof(UsersController.Login), logCommand);

        var logResponse = await _userClient.PostAsync(loginRequest);

        var token = logResponse.Deserialize<LogUserResponse>();

        return token!.Token;
    }
}