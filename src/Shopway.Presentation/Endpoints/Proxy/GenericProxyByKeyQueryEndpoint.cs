using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Endpoints.Proxy;

public sealed class GenericProxyByKeyQueryEndpoint(ISender sender, IMediatorProxyService genericMappingService)
    : Endpoint<GenericProxyByKeyQuery, Results<Ok<DataTransferObjectResponse>, ProblemHttpResult>>
{
    private readonly ISender _sender = sender;
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;

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

    public override async Task<Results<Ok<DataTransferObjectResponse>, ProblemHttpResult>> ExecuteAsync(GenericProxyByKeyQuery query, CancellationToken cancellationToken)
    {
        var queryResult = _genericMappingService.GenericMap(query);

        if (queryResult!.IsFailure)
        {
            return queryResult.ToProblemHttpResult();
        }

        var result = await _sender.Send(queryResult.Value, cancellationToken);

        return result!.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
