using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy.GenericQuery;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.GenericProxy;
using Shopway.Presentation.Utilities;
using System.Security.Claims;

namespace Shopway.Presentation.MinimalEndpoints.Proxy;

public sealed class GenericProxyByKeyQueryMinimalEndpoint : IEndpoint<ProxyGroup>
{
    private const string _name = nameof(GenericProxyByKeyQueryMinimalEndpoint);
    private const string _summary = "Gets entity by specified unique key";
    private const string _description = "Generic proxy query that allows to specify entity type and its desired properties";

    public static void RegisterEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/query/key/generic", GenericProxyByKeyQuery)
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithVersion(VersionGroup.Proxy, 2, 0);
    }

    private static async Task<Results<Ok<DataTransferObjectResponse>, ProblemHttpResult, ForbidHttpResult>> GenericProxyByKeyQuery
    (
        [FromBody] GenericProxyByKeyQuery query,
        ISender sender,
        ClaimsPrincipal user,
        IMediatorProxyService genericMappingService,
        IAuthorizationService authorizationService,
        CancellationToken cancellationToken
    )
    {
        var queryResult = genericMappingService.GenericMap(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        var resourse = GenericProxyRequirementResource.From(query);
        var propertiesCheck = IEntityUtilities.ValidateEntityProperties(resourse.Entity, resourse.RequestedProperties);

        if (propertiesCheck!.IsFailure)
        {
            return propertiesCheck.ToProblemHttpResult();
        }

        var authorizationResult = await authorizationService
            .AuthorizeAsync(user, resourse, GenericProxyPropertiesRequirement.PolicyName);

        if (authorizationResult.Succeeded is false)
        {
            return authorizationResult.ToForbidResult();
        }

        var result = await sender.Send(queryResult.Value, cancellationToken);

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
