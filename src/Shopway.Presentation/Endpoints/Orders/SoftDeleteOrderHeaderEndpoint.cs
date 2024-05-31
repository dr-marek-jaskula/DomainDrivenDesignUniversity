using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;
using Shopway.Domain.Orders;
using Shopway.Presentation.Authentication.OrderHeaders.OrderHeaderCreatedByUser;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.Endpoints.Orders;

public sealed class SoftDeleteOrderHeaderEndpoint(ISender sender, IAuthorizationService authorizationService)
    : EndpointWithoutRequest<Results<Ok, ProblemHttpResult, ForbidHttpResult>>
{
    private readonly ISender _sender = sender;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    private const string _name = nameof(SoftDeleteOrderHeaderEndpoint);
    private const string _summary = "Mark order header as deleted";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation";

    public override void Configure()
    {
        Delete("{id}");

        Group<OrderHeadersGroup>();

        Options(builder => builder
            .WithName(_name)
            .WithDescription(_description)
            .WithSummary(_summary)
            .WithVersion(VersionGroup.OrderHeaders, 1, 0));
    }

    public override async Task<Results<Ok, ProblemHttpResult, ForbidHttpResult>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var id = Route<Ulid>("id");
        var orderHeaderId = OrderHeaderId.Create(id);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, orderHeaderId, OrderHeaderCreatedByUserRequirement.PolicyName);

        if (authorizationResult.Succeeded is false)
        {
            return authorizationResult.ToForbidResult();
        }

        var command = new SoftDeleteOrderHeaderCommand(orderHeaderId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
