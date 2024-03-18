using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.OrderHeaders.OrderHeaderCreatedByUser;

public sealed class OrderHeaderCreatedByUserRequirement : IAuthorizationRequirement
{
    public const string PolicyName = nameof(OrderHeaderCreatedByUser);
}