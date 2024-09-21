﻿using Shopway.SourceGenerator.Base.Tests.Unit.Utilities;
using Shopway.Tests.SourceGenerator.Generators;
using static Shopway.SourceGenerator.Base.Tests.Unit.Utilities.Constants;

namespace Shopway.Tests.SourceGenerator.Tests.Unit.GeneratorTests;

[Trait(nameof(UnitTest), UnitTest.SourceGenerator)]
public sealed class IntegrationTestTraitsGeneratorTests
{
    private readonly IntegrationTestTraitsGenerator _traitsGenerator;

    public IntegrationTestTraitsGeneratorTests()
    {
        _traitsGenerator = new IntegrationTestTraitsGenerator();
    }

    [Fact]
    public void TraitsGenerator_ShouldGenerateTraitsAttribute()
    {
        //Act
        var actualResult = _traitsGenerator.Generate(string.Empty);

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
/// Add to AssemblyReference in the test assembly to generate Integration Test Traits
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = ""Generated by the dr-marek-jaskula source generator."")]
public class GenerateIntegrationTestTraitsAttribute : global::System.Attribute;");
    }

    [Fact]
    public void TraitsGenerator_ShouldGenerateTraits()
    {
        //Arrange
        (string input, string output) = GetTraitsToGenerate();

        //Act
        var actualResult = _traitsGenerator.Generate(input);

        //Assert
        actualResult.Should().Contain(output);
    }

    private static (string input, string output) GetTraitsToGenerate()
    {
        var input = """
        using System;
        
        namespace MyNamespace;
        
        [GenerateIntegrationTestTraitsAttribute]
        public static class AssemblyReference;
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
        
        using Xunit.Abstractions;
        using Xunit.Sdk;
        
        namespace MyNamespace;
        
        public static class IntegrationTest
        {
            public class Api : IntegrationTestAttribute
            {
                public override string IntegrationTest => nameof(Api);
            }
            
            public class PublicApi : IntegrationTestAttribute
            {
                public override string IntegrationTest => nameof(PublicApi);
            }
            
            public class CleanDatabase : IntegrationTestAttribute
            {
                public override string IntegrationTest => nameof(CleanDatabase);
            }
        }
        
        [TraitDiscoverer("MyNamespace.IntegrationTestDiscoverer", "MyNamespace")]
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
        public abstract class IntegrationTestAttribute : Attribute, ITraitAttribute
        {
            public abstract string IntegrationTest { get; }
        }
        
        public sealed class IntegrationTestDiscoverer : ITraitDiscoverer
        {
            public const string Key = nameof(IntegrationTestAttribute.IntegrationTest);
        
            public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
            {
                var category = traitAttribute.GetNamedArgument<string>(Key);
                yield return new KeyValuePair<string, string>(Key, category);
            }
        }
        """; ;

        return (input, output);
    }
}
