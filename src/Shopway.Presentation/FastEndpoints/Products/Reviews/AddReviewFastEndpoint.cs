using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Features.Products.Commands.AddReview;
using Shopway.Domain.Products;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.FastEndpoints.Products.Reviews;

public sealed class AddReviewFastEndpoint(ISender sender)
    : Endpoint<AddReviewCommand.AddReviewRequestBody, Results<Ok<AddReviewResponse>, ProblemHttpResult>>
{
    private readonly ISender _sender = sender;

    private const string _name = nameof(AddReviewFastEndpoint);
    private const string _summary = "Add review to product";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation";

    public override void Configure()
    {
        Post("{productId}/Reviews");

        Group<ProductsGroup>();

        Options(builder => builder
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithMetadata(new RequiredPermissionsAttribute(Domain.Common.Enums.LogicalOperation.Or, PermissionName.Review_Add, PermissionName.INVALID_PERMISSION))
            .WithVersion(VersionGroup.Products, 1, 0));
    }

    public override async Task<Results<Ok<AddReviewResponse>, ProblemHttpResult>> ExecuteAsync(AddReviewCommand.AddReviewRequestBody body, CancellationToken cancellationToken)
    {
        var productId = Route<Ulid>("productId");

        var command = new AddReviewCommand(ProductId.Create(productId), body);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
