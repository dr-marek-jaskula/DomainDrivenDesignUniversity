﻿# Test Source Generator :musical_keyboard:

This source generator is dedicated for Shopway Test projects. It is configured to generate xunit testing capabilities like custom Traits. 

Nevertheless, feel free to use or/and modify the code for Your own purpose. The IncrementalGeneratorBase and some utilities should be generic enough to be highly
reusable.

# How to use?

The default version is already packed to nuget, but if some updates are required we need to:
1. Increment the version **Version** in project file
2. Use command ```dotnet pack -c Release -o ./..``` at Shopway.SourceGenerator directory. You can specify other output directory and than copy it to SourceGenerators folder.

**IMPORTANT**: remember to use ```-c Release```.

Example of auto-generated entity id:

```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the dr-marek-jaskula source generator
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Diagnostics;

namespace Shopway.Domain.Products;

[DebuggerDisplay("{Value}")]
public readonly record struct ProductId : IEntityId<ProductId>
{
    public const string Name = "ProductId";
    public const string Namespace = "Shopway.Domain.Products";

    private ProductId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; }

    public static ProductId New()
    {
        return new ProductId(Ulid.NewUlid());
    }

    public static ProductId Create(Ulid id)
    {
        return new ProductId(id);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public int CompareTo(IEntityId? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is not ProductId otherId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherId.Value);
    }

    public static bool operator >(ProductId a, ProductId b) => a.CompareTo(b) is 1;
    public static bool operator <(ProductId a, ProductId b) => a.CompareTo(b) is -1;
    public static bool operator >=(ProductId a, ProductId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(ProductId a, ProductId b) => a.CompareTo(b) <= 0;
}

```

## Testability

Source generator are hard do debug. Therefore, a very important part of creating a source generator are tests. 
They need a specific setup, so to examine it, see **Shopway.SourceGenerator.Tests.Unit**.

## Generic Parameters for Attributes

The desired approach would be to use type parameter:

```csharp
public class GenerateEntityIdConverterAttribute<TValue> : global::System.Attribute;
```

However the Roslyn source generator seems not to support that option. Therefore, we have:

```csharp
public class GenerateEntityIdConverterAttribute : global::System.Attribute
{
    public required string {{{IdName}}};
    public required string {{{IdNamespace}}};
}
```