using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Net.Mime;

namespace Shopway.Presentation.Abstractions;

/// <summary>
/// Automatically registered in MinimalApiRegistration
/// </summary>
public sealed class BaseEndpointGroup : IEndpointGroup
{
    public static IEndpointRouteBuilder RegisterEndpointGroup(IEndpointRouteBuilder app)
    {
        return app.MapGroup("/api")
            .WithMetadata(new ProducesResponseTypeAttribute<ProblemDetails>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson))
            .WithOpenApi();
    }
}
