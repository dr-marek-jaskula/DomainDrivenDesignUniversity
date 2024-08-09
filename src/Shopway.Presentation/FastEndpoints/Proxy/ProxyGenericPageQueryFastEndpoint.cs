using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Presentation.Authentication.GenericProxy;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.FastEndpoints.Proxy;

public sealed class ProxyGenericPageQueryFastEndpoint(ISender sender, IMediatorProxyService genericMappingService, IAuthorizationService authorizationService)
    : Endpoint<GenericProxyPageQuery, Results<Ok<object>, ProblemHttpResult, ForbidHttpResult>>
{
    private readonly ISender _sender = sender;
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    private const string _name = nameof(ProxyGenericPageQueryFastEndpoint);
    private const string _summary = "Gets entities for specified pagination settings: offset or cursor";
    private const string _description = "Generic proxy page query that allows to specify entity type and its desired properties. Filtering and sorting is supported";

    public override void Configure()
    {
        Post("query/page/generic");

        Group<ProxyGroup>();

        Options(builder => builder
            .Produces<PageResponse<DataTransferObjectResponse>>(StatusCodes.Status200OK)
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithVersion(VersionGroup.Proxy, 2, 0));
    }

    public override async Task<Results<Ok<object>, ProblemHttpResult, ForbidHttpResult>> ExecuteAsync(GenericProxyPageQuery query, CancellationToken cancellationToken)
    {
        var queryResult = _genericMappingService.GenericMap(query);

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

        var authorizationResult = await _authorizationService
            .AuthorizeAsync(User, resourse, GenericProxyPropertiesRequirement.PolicyName);

        if (authorizationResult.Succeeded is false)
        {
            return authorizationResult.ToForbidResult();
        }

        object concretePageQuery = queryResult.Value;

        var result = await _sender.Send(concretePageQuery, cancellationToken) as Domain.Common.Results.IResult<object>;

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
