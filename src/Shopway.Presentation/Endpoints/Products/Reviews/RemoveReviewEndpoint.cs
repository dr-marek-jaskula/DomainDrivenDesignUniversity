using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Features.Products.Commands.RemoveReview;
using Shopway.Domain.Enums;
using Shopway.Domain.Products;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.Endpoints.Products.Reviews;

public class RemoveReviewRequest
{
    public Ulid ProductId { get; set; }
    public Ulid ReviewId { get; set; }
}

public sealed class RemoveReviewEndpoint(ISender sender)
    : Endpoint<RemoveReviewRequest, Results<Ok<RemoveReviewResponse>, ProblemHttpResult>>
{
    private readonly ISender _sender = sender;

    private const string _name = nameof(RemoveReviewEndpoint);
    private const string _summary = "Remove review from product";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation";

    public override void Configure()
    {
        Delete("{@productId}/Reviews/{@reviewId}", request => new { request.ProductId, request.ReviewId });

        Group<ProductsGroup>();

        Options(builder => builder
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithMetadata(new RequiredPermissionsAttribute(Permission.Review_Remove))
            .WithMetadata(new RequiredRolesAttribute(Role.Administrator))
            .WithVersion(VersionGroup.Products, 1, 0));
    }

    public override async Task<Results<Ok<RemoveReviewResponse>, ProblemHttpResult>> ExecuteAsync(RemoveReviewRequest request, CancellationToken cancellationToken)
    {
        var command = new RemoveReviewCommand(ProductId.Create(request.ProductId), ReviewId.Create(request.ReviewId));

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
