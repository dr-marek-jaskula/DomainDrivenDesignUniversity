﻿using Microsoft.EntityFrameworkCore;
using RestSharp;
using RestSharp.Authenticators;
using Shopway.Application.Features.Users.Commands.RegisterUser;
using Shopway.Application.Features.Users.Commands.LogUser;
using Shopway.Domain.Entities;
using Shopway.Tests.Integration.Persistence;
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

public abstract class ControllerTestsBase : IDisposable
{
    private readonly string ShopwayApiUrl;
    private readonly RestClient _userClient;
    protected readonly string _controllerUri;
    protected readonly IServiceScope Scope;
    protected readonly ApiKeyTestOptions apiKeys;
    protected readonly IntegrationTestsUrlOptions integrationTestsUrlOptions;

    public ControllerTestsBase(DependencyInjectionContainerTestFixture containerTestFixture)
    {
        Scope = containerTestFixture
            .ServiceProvider
            .CreateScope();

        integrationTestsUrlOptions = (IntegrationTestsUrlOptions)containerTestFixture
            .ServiceProvider
            .GetRequiredService(typeof(IntegrationTestsUrlOptions));

        apiKeys = (ApiKeyTestOptions)containerTestFixture
            .ServiceProvider
            .GetRequiredService(typeof(ApiKeyTestOptions));

        ShopwayApiUrl = integrationTestsUrlOptions.ShopwayApiUrl!;
        _controllerUri = GetType().Name[..^ControllerTests.Length];
        _userClient = new($"{ShopwayApiUrl}{nameof(UsersController)[..^Controller.Length]}");
    }

    /// <summary>
    /// Create the rest client with api url appended by given controller url and ensure that the test user for this client has all privileges
    /// </summary>
    /// <param name="controllerUrl">Controller url</param>
    /// <param name="databaseFixture">Database fixture</param>
    /// <returns></returns>
    protected async Task<RestClient> RestClient(string controllerUrl, DatabaseFixture databaseFixture)
    {
        await EnsureThatTheTestUserIsRegistered(databaseFixture);
        var token = await LogTestUser();

        var restClientOptions = new RestClientOptions(new Uri($"{ShopwayApiUrl}{controllerUrl}"))
        {
            Authenticator = new JwtAuthenticator(token)
        };

        return new RestClient(restClientOptions);
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

    public virtual void Dispose()
    {
        Scope.Dispose();
    }
}