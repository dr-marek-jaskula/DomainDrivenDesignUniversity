using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Features.Products.Queries;
using Shopway.Application.Features.Products.Queries.GetProductById;
using Shopway.Domain.Products;
using Shopway.Presentation.Authentication;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.FastEndpoints.Products;

public sealed class GetProductByIdFastEndpoint(ISender sender)
    : EndpointWithoutRequest<Results<Ok<ProductResponse>, ProblemHttpResult>>
{
    private readonly ISender _sender = sender;

    private const string _name = nameof(GetProductByIdFastEndpoint);
    private const string _summary = "Gets product by specified id";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation";

    public override void Configure()
    {
        Get("{id}");

        Group<ProductsGroup>();

        Options(builder => builder
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithMetadata(new RequiredApiKeyAttribute(RequiredApiKey.PRODUCT_GET))
            .WithVersion(VersionGroup.Products, 1, 0));

        AuthSchemes(AnonymousSchema.Name);
    }

    public override async Task<Results<Ok<ProductResponse>, ProblemHttpResult>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var id = Route<Ulid>("id");

        var query = new GetProductByIdQuery(ProductId.Create(id));

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
