using System.Text;
using static Shopway.SourceGenerator.Base.Utilities.Constants;

namespace Shopway.Tests.SourceGenerator.Generators;

public readonly record struct IntegrationTestTraitsToGenerateEntry
{
    private const string XUnitAbstractions = "using Xunit.Abstractions;";
    private const string XUnitSdkNamespace = "using Xunit.Sdk;";

    public readonly string Namespace;

    public static readonly IntegrationTestTraitsToGenerateEntry None = new(string.Empty);

    public IntegrationTestTraitsToGenerateEntry(string @namespace)
    {
        Namespace = @namespace;
    }

    public string Generate(StringBuilder stringBuilder)
    {
        return stringBuilder
            .Append(Header)
            .AppendLine()
            .AppendLine(XUnitAbstractions)
            .AppendLine(XUnitSdkNamespace)
            .AppendLine()
            .AppendLine($"namespace {Namespace};")
            .AppendLine()
            .AppendLine(AppendIntegrationTests())
            .AppendLine()
            .ToString();
    }

    public string AppendIntegrationTests()
    {
        return $$$"""
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

        [TraitDiscoverer("{{{Namespace}}}.IntegrationTestDiscoverer", "{{{Namespace}}}")]
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
        """;
    }
}
