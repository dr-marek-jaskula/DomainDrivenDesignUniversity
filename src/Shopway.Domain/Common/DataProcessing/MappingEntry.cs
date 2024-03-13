using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.DataProcessing;

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

    public List<string> GetAllPropertyNames()
    {
        var names = new List<string>();

        if (PropertyName is not null)
        {
            names.Add(PropertyName);
        }

        if (From is not null)
        {
            names.Add(From);

            if (Properties is not null)
            {
                foreach (var property in Properties)
                {
                    if (property is null)
                    {
                        names.Add("NULL");
                        continue;
                    }

                    names.AddRange(property.GetAllPropertyNames());
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