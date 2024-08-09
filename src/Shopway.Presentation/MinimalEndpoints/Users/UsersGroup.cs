using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.MinimalEndpoints.Users;

public sealed class UsersGroup : IEndpointGroup
{
    private const string Group = "Users";
    private const string Tag = $"{Group}.MinimalEndpoints";

    public static IEndpointRouteBuilder RegisterEndpointGroup(IEndpointRouteBuilder app)
    {
        return app.MapGroup($"/minimal/{Group}")
            .AllowAnonymous()
            .WithTags(Tag);
    }
}
