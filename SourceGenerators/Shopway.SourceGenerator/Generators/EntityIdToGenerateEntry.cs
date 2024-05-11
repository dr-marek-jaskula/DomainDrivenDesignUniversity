namespace Shopway.SourceGenerator.Generators;

public readonly record struct EntityIdToGenerateEntry
{
    public readonly string Name;
    public readonly string Namespace;

    public static readonly EntityIdToGenerateEntry None = new(string.Empty, string.Empty);

    public EntityIdToGenerateEntry
    (
        string name,
        string @namespace
    )
    {
        Name = name;
        Namespace = @namespace;
    }
}