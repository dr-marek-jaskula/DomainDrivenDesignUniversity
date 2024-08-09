using Microsoft.AspNetCore.Routing;

namespace Shopway.Presentation.Abstractions;

public interface IEndpoint<TEndpointGroup> : IEndpoint
    where TEndpointGroup : class, IEndpointGroup, new();

public interface IEndpoint
{
    static abstract void RegisterEndpoint(IEndpointRouteBuilder app);
}
