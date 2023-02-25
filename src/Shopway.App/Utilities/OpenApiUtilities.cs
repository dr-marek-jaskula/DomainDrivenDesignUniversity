using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shopway.App.Utilities;

public static class OpenApiUtilities
{
    /// <summary>
    /// Used to add to OpenApi a new custom header
    /// </summary>
    /// <param name="options"></param>
    /// <param name="headerName">Name of the header</param>
    /// <param name="headerDescription">Header description</param>
    /// <param name="isHeaderRequiredInOpenApi">True if header is required, false otherwise. Default false</param>
    public static void AddCustomHeader(this SwaggerGenOptions options, string headerName, string headerDescription, bool isHeaderRequiredInOpenApi = false)
    {
        options.OperationFilter<AddHeaderOperationFilter>(headerName, headerDescription, isHeaderRequiredInOpenApi);
        options.OperationFilter<AddResponseHeadersFilter>();
    }
}
