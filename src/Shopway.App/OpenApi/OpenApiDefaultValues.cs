using System.Text.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Microsoft.Extensions.DependencyInjection;

public sealed class OpenApiDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters is null)
        {
            return;
        }

        foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse 
                ? "default" 
                : responseType.StatusCode.ToString();

            var response = operation.Responses[responseKey];

            foreach (var contentType in response.Content.Keys)
            {
                if (responseType.ApiResponseFormats.Any(x => x.MediaType == contentType) is false)
                {
                    response.Content.Remove(contentType);
                }
            }
        }

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter.Schema.Default is null && description.DefaultValue is not null)
            {
                if (description.ModelMetadata is not null)
                {
                    var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                }
                else
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }
            }

            parameter.Required |= description.IsRequired;

        }
    }
}