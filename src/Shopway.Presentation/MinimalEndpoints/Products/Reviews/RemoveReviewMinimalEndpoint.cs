using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Shopway.Application.Features.Products.Commands.RemoveReview;
using Shopway.Domain.Products;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.MinimalEndpoints.Products.Reviews;

public sealed class RemoveReviewMinimalEndpoint : IEndpoint<ProductsGroup>
{
    private const string _name = nameof(RemoveReviewMinimalEndpoint);
    private const string _summary = "Remove review from product";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation";

    public static void RegisterEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{productId}/Reviews/{reviewId}", AddReview)
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithMetadata(new RequiredPermissionsAttribute(PermissionName.Review_Remove))
            .WithMetadata(new RequiredRolesAttribute(RoleName.Administrator))
            .WithVersion(VersionGroup.Products, 1, 0);
    }

    private static async Task<Results<Ok<RemoveReviewResponse>, ProblemHttpResult, ForbidHttpResult>> AddReview
    (
        ProductId productId,
        ReviewId reviewId,
        ISender sender,
        CancellationToken cancellationToken
    )
    {
        var command = new RemoveReviewCommand(productId, reviewId);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
