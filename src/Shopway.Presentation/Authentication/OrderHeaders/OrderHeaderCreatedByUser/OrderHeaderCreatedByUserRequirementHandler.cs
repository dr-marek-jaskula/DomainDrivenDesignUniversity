using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Orders;
using Shopway.Domain.Users;
using System.Security.Claims;

namespace Shopway.Presentation.Authentication.OrderHeaders.OrderHeaderCreatedByUser;

public sealed class OrderHeaderCreatedByUserRequirementHandler(IOrderHeaderRepository orderHeaderRepository) 
    : AuthorizationHandler<OrderHeaderCreatedByUserRequirement, OrderHeaderId>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderHeaderCreatedByUserRequirement requirement, OrderHeaderId orderHeaderId)
    {
        var parseResult = Ulid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userIdAsUlid);

        if (parseResult is false)
        {
            context.Fail();
            return;
        }

        var isOrderHeaderCreatedByUser = await orderHeaderRepository
            .IsOrderHeaderCreatedByUser(orderHeaderId, UserId.Create(userIdAsUlid), CancellationToken.None);

        if (isOrderHeaderCreatedByUser is false)
        {
            context.Fail(new AuthorizationFailureReason(this, "User can delete only its own orderHeader"));
            return;
        }

        context.Succeed(requirement);
    }
}