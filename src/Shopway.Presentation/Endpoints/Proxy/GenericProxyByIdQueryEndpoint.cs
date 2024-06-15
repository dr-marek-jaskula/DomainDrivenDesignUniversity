using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.Endpoints.Proxy;

public sealed class GenericProxyByIdQueryEndpoint(ISender sender, IMediatorProxyService genericMappingService)
    : Endpoint<GenericProxyByIdQuery, Results<Ok<DataTransferObjectResponse>, ProblemHttpResult>>
{
    private readonly ISender _sender = sender;
    private readonly IMediatorProxyService _genericMappingService = genericMappingService;

    private const string _name = nameof(GenericProxyByIdQueryEndpoint);
    private const string _summary = "Gets entity by specified id";
    private const string _description = "Generic proxy query that allows to specify entity type and its desired properties";

    public override void Configure()
    {
        Post("query/id/generic");

        Group<ProxyGroup>();

        Options(builder => builder
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithVersion(VersionGroup.Proxy, 2, 0));
    }

    public override async Task<Results<Ok<DataTransferObjectResponse>, ProblemHttpResult>> ExecuteAsync(GenericProxyByIdQuery query, CancellationToken cancellationToken)
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
