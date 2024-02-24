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
                    names.AddRange(property.GetAllPropertyNames());
                }
            }
        }

        return names;
    }
}