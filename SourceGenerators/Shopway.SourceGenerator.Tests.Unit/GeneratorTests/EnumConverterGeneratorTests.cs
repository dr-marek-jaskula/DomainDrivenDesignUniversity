﻿using Shopway.SourceGenerator.Generators;
using Shopway.SourceGenerator.Tests.Unit.Utilities;
using static Shopway.SourceGenerator.Tests.Unit.Utilities.Constants;

namespace Shopway.SourceGenerator.Tests.Unit.GeneratorTests;

[Trait(nameof(UnitTest), UnitTest.SourceGenerator)]
public sealed class EnumConverterGeneratorTests
{
    private readonly EnumConverterGenerator _enumConverterGenerator;

    public EnumConverterGeneratorTests()
    {
        _enumConverterGenerator = new EnumConverterGenerator();
    }

    [Fact]
    public void EnumConverterGenerator_ShouldGenerateEnumConverterAttribute()
    {
        //Act
        var actualResult = _enumConverterGenerator.Generate(string.Empty);

        //Assert
        actualResult.Should().Be(@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the dr-marek-jaskula source generator
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

namespace System;

/// <summary>
/// Add to custom empty class for given enum to indicate that enum converter should be generated
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = ""Generated by the dr-marek-jaskula source generator."")]
public class GenerateEnumConverterAttribute : global::System.Attribute
{
    public required string EnumName;
    public required string EnumNamespace;
}");
    }

    [Fact]
    public void EnumConverterGenerator_ShouldGenerateEnumConverter()
    {
        //Arrange
        (string input, string output) = GetEnumToGenerateId();

        //Act
        var actualResult = _enumConverterGenerator.Generate(input);

        //Asset
        actualResult.Should().Be(output);
    }

    private static (string input, string output) GetEnumToGenerateId()
    {
        var input = """
        using System;
        using OtherNamespace.EnumNamespace;
        
        namespace MyNamespace
        {
            [GenerateEnumConverter(EnumName = nameof(ExecutionStatus), EnumNamespace = "OtherNamespace.EnumNamespace"]
            public sealed class GenerateEnumConverters;
        }
        
        namespace OtherNamespace.EnumNamespace
        {
            public enum ExecutionStatus
            {
                InProgress = 0,
                Failure = 1,
                Success = 2
            }
        }
        """;

        var output = """
         //------------------------------------------------------------------------------
         // <auto-generated>
         //     This code was generated by the dr-marek-jaskula source generator
         //
         //     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
         // </auto-generated>
         //------------------------------------------------------------------------------

         #nullable enable

         using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
         using OtherNamespace.EnumNamespace;

         namespace MyNamespace;

         public sealed class ExecutionStatusConverter : ValueConverter<ExecutionStatus, string>
         {
             public ExecutionStatusConverter() : base(status => status.ToString(), @string => (ExecutionStatus)Enum.Parse(typeof(ExecutionStatus), @string)) { }
         }

         """;

        return (input, output);
    }
}
