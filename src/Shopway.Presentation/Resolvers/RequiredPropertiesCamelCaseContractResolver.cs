using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using static Newtonsoft.Json.Required;

namespace Shopway.Presentation.Resolvers;

public sealed class RequiredPropertiesCamelCaseContractResolver : CamelCasePropertyNamesContractResolver
{
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);

        foreach (var contractProperty in contract.Properties)
        {
            if (contractProperty?.PropertyType?.IsValueType is true
                && contractProperty?.AttributeProvider?.GetAttributes(typeof(RequiredAttribute), inherit: true).Count > 0)
            {
                contractProperty.Required = Always;
            }
        }

        return contract;
    }
}