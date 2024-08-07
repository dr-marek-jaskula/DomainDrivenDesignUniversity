﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Shopway.Domain.Common.Utilities;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection;

public sealed class OpenApiDefaultValues : IOperationFilter
{
    private const string id = "id";
    private const string @Default = "default";
    private const string UlidExample = "\"01AN4Z07BY79KA1307SR9X4MV3\"";

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
                ? @Default
                : responseType.StatusCode.ToString();

            var response = operation.Responses[responseKey];

            var contentKeysToRemove = response.Content.Keys
                .Where(contentType => responseType.ApiResponseFormats.Any(x => x.MediaType == contentType) is false);

            foreach (var contentType in contentKeysToRemove)
            {
                response.Content.Remove(contentType);
            }
        }

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter.Schema.Default is null && description.DefaultValue.NotNullOrEmptyObject())
            {
                if (description.ModelMetadata is not null)
                {
                    var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                }
                else
                {
                    parameter.Schema.Default = new OpenApiString($"{description.DefaultValue}");
                }
            }

            parameter.Required |= description.IsRequired;

            UseUlidExampleForEntityIdsInTheRoute(parameter);
        }
    }

    /// <summary>
    /// Due to the fact the entity id is a strongly type id, swagger example is an json object. 
    /// To handle this problem we add a custom example for an entity id that is a constant ulid
    /// </summary>
    /// <param name="parameter">Open api parameter</param>
    private static void UseUlidExampleForEntityIdsInTheRoute(OpenApiParameter? parameter)
    {
        if (parameter is not null && parameter.Name.Contains(id, StringComparison.OrdinalIgnoreCase))
        {
            parameter.Examples.TryAdd(parameter.Name, new OpenApiExample
            {
                Value = new OpenApiString(UlidExample)
            });
        }
    }
}
