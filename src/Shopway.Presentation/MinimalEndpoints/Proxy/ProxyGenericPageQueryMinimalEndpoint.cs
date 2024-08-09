using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.GenericProxy;
using Shopway.Presentation.Utilities;
using System.Security.Claims;

namespace Shopway.Presentation.MinimalEndpoints.Proxy;

public sealed class ProxyGenericPageQueryMinimalEndpoint : IEndpoint<ProxyGroup>
{
    private const string _name = nameof(ProxyGenericPageQueryMinimalEndpoint);
    private const string _summary = "Gets entities for specified pagination settings: offset or cursor";
    private const string _description = "Generic proxy page query that allows to specify entity type and its desired properties. Filtering and sorting is supported";

    public static void RegisterEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/query/page/generic", GenericProxyByKeyQuery)
            .Produces<PageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithVersion(VersionGroup.Proxy, 2, 0);
    }

    private static async Task<Results<Ok<object>, ProblemHttpResult, ForbidHttpResult>> GenericProxyByKeyQuery
    (
        [FromBody] GenericProxyPageQuery query,
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

        object concretePageQuery = queryResult.Value;

        var result = await sender.Send(concretePageQuery, cancellationToken) as Domain.Common.Results.IResult<object>;

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
