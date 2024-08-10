using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Shopway.Application.Features.Products.Queries;
using Shopway.Application.Features.Products.Queries.GetProductById;
using Shopway.Domain.Products;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.MinimalEndpoints.Products;

public sealed class GetProductByIdMinimalEndpoint : IEndpoint<ProductsGroup>
{
    private const string _name = nameof(GetProductByIdMinimalEndpoint);
    private const string _summary = "Gets product by specified id";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation";

    public static void RegisterEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", GetProductById)
            .WithName(_name)
            .WithDescription(_description)
            .WithSummaryApiKey(_summary)
            .WithMetadata(new RequiredApiKeyAttribute<ApiKey>(ApiKey.PRODUCT_GET))
            .WithVersion(VersionGroup.Products, 1, 0);
    }

    private static async Task<Results<Ok<ProductResponse>, ProblemHttpResult, ForbidHttpResult>> GetProductById(ProductId id, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
