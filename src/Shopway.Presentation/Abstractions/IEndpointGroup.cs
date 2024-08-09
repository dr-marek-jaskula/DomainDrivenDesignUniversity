using Microsoft.AspNetCore.Routing;

namespace Shopway.Presentation.Abstractions;

public interface IEndpointGroup
{
    static abstract IEndpointRouteBuilder RegisterEndpointGroup(IEndpointRouteBuilder app);
}
