using Microsoft.EntityFrameworkCore;
using RestSharp;
using RestSharp.Authenticators;
using Shopway.Application.CQRS.Users.Commands.CreateUser;
using Shopway.Application.CQRS.Users.Commands.LogUser;
using Shopway.Domain.Entities;
using Shopway.Tests.Integration.Helpers;
using Shopway.Tests.Integration.Persistance;
using Shopway.Tests.Integration.Utilities;
using static RestSharp.Method;

namespace Shopway.Tests.Integration.Abstractions;

public abstract partial class ControllerTestsBase
{
    private const string ShopwayApiUrl = "https://localhost:7236/api/";
    protected readonly string _controllerUri;
    private static readonly RestClient _userClient = new($"{ShopwayApiUrl}{"user"}");

    public ControllerTestsBase()
	{
        _controllerUri = GetType().Name[..^"ControllerTests".Length];
    }

    protected static async Task<RestClient> RestClient(string controllerUri, DatabaseFixture databaseFixture)
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
    private static async Task EnsureThatTheTestUserIsRegistered(DatabaseFixture databaseFixture)
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

        var registerRequest = PostRequest("register", registerCommand);

        await _userClient.PostAsync(registerRequest);

        var user = databaseFixture
            .Context
            .Set<User>()
            .First();

        await databaseFixture.Context.Database.ExecuteSqlRawAsync(@$"
            INSERT INTO Master.RoleUser (RoleId, UserId)
            VALUES (1, '{user.Id.Value}');     
            ");

        await databaseFixture.Context.SaveChangesAsync();
    }

    /// <summary>
    /// Register and log the test user
    /// </summary>
    /// <param name="databaseFixture"></param>
    /// <returns>Jwt token</returns>
    private static async Task<string> LogTestUser()
    {
        var logCommand = new LogUserCommand(TestUser.Email, TestUser.Password);

        var loginRequest = PostRequest("login", logCommand);

        var logResponse = await _userClient.PostAsync(loginRequest);

        var token = logResponse.Deserialize<LogUserResponse>();

        return token!.Token;
    }
}