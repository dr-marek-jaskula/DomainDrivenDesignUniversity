using Microsoft.EntityFrameworkCore;
using RestSharp;
using RestSharp.Authenticators;
using Shopway.Application.CQRS.Users.Commands.CreateUser;
using Shopway.Application.CQRS.Users.Commands.LogUser;
using Shopway.Domain.Entities;
using Shopway.Tests.Integration.Persistance;
using Shopway.Tests.Integration.Utilities;
using static RestSharp.Method;

namespace Shopway.Tests.Integration.Abstractions;

public abstract partial class ControllerTestsBase
{
    private const string ShopwayApiUrl = "https://localhost:7236/api/";
    protected readonly string _controllerUri;
    protected readonly string _userControllerUri = "user";

    public ControllerTestsBase()
	{
        _controllerUri = GetType().Name[..^"ControllerTests".Length];
    }

    protected async Task<RestClient> RestClient(string controllerUri, DatabaseFixture databaseFixture)
	{
        var client = new RestClient($"{ShopwayApiUrl}{controllerUri}");

        var token = await RegisterAndLogTestUser(databaseFixture);

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
    /// Register and log the test user
    /// </summary>
    /// <param name="databaseFixture"></param>
    /// <returns>Jwt token</returns>
    private async Task<string> RegisterAndLogTestUser(DatabaseFixture databaseFixture)
    {
        var userClient = new RestClient($"{ShopwayApiUrl}{_userControllerUri}");

        var registerCommand = new CreateUserCommand("TestUser", "testUser123@gmail.com", "testPassword123!", "testPassword123!");

        var registerRequest = PostRequest("register", registerCommand);

        await userClient.PostAsync(registerRequest);

        var user = databaseFixture
            .Context
            .Set<User>()
            .First();

        await databaseFixture.Context.Database.ExecuteSqlRawAsync(@$"
            INSERT INTO Master.RoleUser (RoleId, UserId)
            VALUES (1, '{user.Id.Value}');     
            ");

        await databaseFixture.Context.SaveChangesAsync();

        var logCommand = new LogUserCommand("testUser123@gmail.com", "testPassword123!");

        var loginRequest = PostRequest("login", logCommand);

        var logResponse = await userClient.PostAsync(loginRequest);

        var token = logResponse.Deserialize<LogUserResponse>();

        return token!.Token;
    }
}