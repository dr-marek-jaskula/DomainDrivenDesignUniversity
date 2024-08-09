using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Shopway.Application.Features.Products.Commands.AddReview;
using Shopway.Domain.Products;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.MinimalEndpoints.Products.Reviews;

public sealed class AddReviewMinimalEndpoint : IEndpoint<ProductsGroup>
{
    private const string _name = nameof(AddReviewMinimalEndpoint);
    private const string _summary = "Add review to product";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation";

    public static void RegisterEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/{productId}/Reviews", AddReview)
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithMetadata(new RequiredPermissionsAttribute(Domain.Common.Enums.LogicalOperation.Or, PermissionName.Review_Add, PermissionName.INVALID_PERMISSION))
            .WithVersion(VersionGroup.Products, 1, 0);
    }

    private static async Task<Results<Ok<AddReviewResponse>, ProblemHttpResult, ForbidHttpResult>> AddReview
    (
        ProductId productId,
        ISender sender,
        AddReviewCommand.AddReviewRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var query = new AddReviewCommand(productId, body);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
