using System.Text;
using static Shopway.SourceGenerator.Base.Utilities.Constants;

namespace Shopway.Tests.SourceGenerator.Generators;

public readonly record struct TraitsToGenerateEntry
{
    private const string XUnitAbstractions = "using Xunit.Abstractions;";
    private const string XUnitSdkNamespace = "using Xunit.Sdk;";

    public readonly string Namespace;

    public static readonly TraitsToGenerateEntry None = new(string.Empty);

    public TraitsToGenerateEntry(string @namespace)
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
            .AppendLine(AppendUnitTests())
            .ToString();
    }

    public string AppendUnitTests()
    {
        return $$$"""
        public static class UnitTest
        {
            public class Utility : UnitTestAttribute
            {
                public override string UnitTest => nameof(Utility);
            }
            
            public class Architecture : UnitTestAttribute
            {
                public override string UnitTest => nameof(Architecture);
            }
            
            public class Domain : UnitTestAttribute
            {
                public override string UnitTest => nameof(Domain);
            }
            
            public class Application : UnitTestAttribute
            {
                public override string UnitTest => nameof(Application);
            }
            
            public class Persistence : UnitTestAttribute
            {
                public override string UnitTest => nameof(Persistence);
            }
            
            public class Infrastructure : UnitTestAttribute
            {
                public override string UnitTest => nameof(Infrastructure);
            }
            
            public class App : UnitTestAttribute
            {
                public override string UnitTest => nameof(App);
            }
            
            public class Presentation : UnitTestAttribute
            {
                public override string UnitTest => nameof(Presentation);
            }
        }

        [TraitDiscoverer("{{{Namespace}}}.UnitTestDiscoverer", "{{{Namespace}}}")]
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
        public abstract class UnitTestAttribute : Attribute, ITraitAttribute
        {
            public abstract string UnitTest { get; }
        }
        
        public sealed class UnitTestDiscoverer : ITraitDiscoverer
        {
            public const string Key = nameof(UnitTestAttribute.UnitTest);
        
            public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
            {
                var category = traitAttribute.GetNamedArgument<string>(Key);
                yield return new KeyValuePair<string, string>(Key, category);
            }
        }
        """;
    }
}
