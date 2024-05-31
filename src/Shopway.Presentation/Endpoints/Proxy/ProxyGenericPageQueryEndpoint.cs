using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Endpoints.Proxy;

public sealed class ProxyGenericPageQueryEndpoint(ISender sender, IMediatorProxyService genericMappingService) 
    : Endpoint<GenericProxyPageQuery, Results<Ok<object>, ProblemHttpResult>>
{
    private readonly ISender _sender = sender;
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;

    private const string _name = nameof(ProxyGenericPageQueryEndpoint);
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

    public override async Task<Results<Ok<object>, ProblemHttpResult>> ExecuteAsync(GenericProxyPageQuery query, CancellationToken cancellationToken)
    {
        var queryResult = _genericMappingService.GenericMap(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        object concretePageQuery = queryResult.Value;

        var result = await _sender.Send(concretePageQuery, cancellationToken) as Shopway.Domain.Common.Results.IResult<object>;

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
