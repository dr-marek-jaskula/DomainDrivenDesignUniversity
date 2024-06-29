using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Presentation.Authentication.GenericProxy;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.Endpoints.Proxy;

public sealed class GenericProxyByKeyQueryEndpoint(ISender sender, IMediatorProxyService genericMappingService, IAuthorizationService authorizationService)
    : Endpoint<GenericProxyByKeyQuery, Results<Ok<DataTransferObjectResponse>, ProblemHttpResult, ForbidHttpResult>>
{
    private readonly ISender _sender = sender;
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    private const string _name = nameof(GenericProxyByKeyQueryEndpoint);
    private const string _summary = "Gets entity by specified unique key";
    private const string _description = "Generic proxy query that allows to specify entity type and its desired properties";

    public override void Configure()
    {
        Post("query/key/generic");

        Group<ProxyGroup>();

        Options(builder => builder
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithVersion(VersionGroup.Proxy, 2, 0));
    }

    public override async Task<Results<Ok<DataTransferObjectResponse>, ProblemHttpResult, ForbidHttpResult>> ExecuteAsync(GenericProxyByKeyQuery query, CancellationToken cancellationToken)
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

        var result = await _sender.Send(queryResult.Value, cancellationToken);

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
