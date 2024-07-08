using Shopway.Domain.Common.Utilities;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Common.DataProcessing;

[JsonConverter(typeof(MappingEntryJsonConverter))]
public sealed class MappingEntry
{
    public string? PropertyName { get; init; }
    public string? From { get; init; }
    public List<MappingEntry>? Properties { get; init; }

    public static explicit operator MappingEntry(string value)
    {
        return new MappingEntry()
        {
            PropertyName = value,
        };
    }

    public List<string> GetAllPropertyNamesWithHierarchy(string? hierarchy = null)
    {
        var names = new List<string>();

        if (PropertyName is not null)
        {
            names.Add(hierarchy is not null ? $"{hierarchy}.{PropertyName}" : PropertyName);
        }

        if (From is not null)
        {
            if (Properties is not null)
            {
                foreach (var property in Properties)
                {
                    if (property is null)
                    {
                        names.Add("NULL");
                        continue;
                    }

                    names.AddRange(property.GetAllPropertyNamesWithHierarchy(hierarchy is not null ? $"{hierarchy}.{From}" : From));
                }
            }
        }

        return names;
    }

    public override string ToString()
    {
        if (PropertyName is not null)
        {
            return $"{PropertyName}";
        }

        return $"From '{From}': {string.Join(',', Properties!.Select(x => $"{x}"))}";
    }
}
