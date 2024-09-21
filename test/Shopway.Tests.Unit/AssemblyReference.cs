//using System.Reflection;
//using Xunit.Abstractions;
//using Xunit.Sdk;

//namespace Shopway.Tests.Unit;

//public static class AssemblyReference
//{
//    public static Assembly Assembly { get; set; } = typeof(AssemblyReference).Assembly;
//}

//public static class UnitTest
//{
//    public class Utility : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(Utility);
//    }

//    public class Architecture : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(Architecture);
//    }

//    public class Domain : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(Domain);
//    }

//    public class Application : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(Application);
//    }

//    public class Persistence : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(Persistence);
//    }

//    public class Infrastructure : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(Infrastructure);
//    }

//    public class App : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(App);
//    }

//    public class Presentation : UnitTestAttribute
//    {
//        public override string UnitTest => nameof(Presentation);
//    }
//}

//[TraitDiscoverer("Shopway.Tests.Unit.UnitTestDiscoverer", "Shopway.Tests.Unit")]
//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
//public abstract class UnitTestAttribute : Attribute, ITraitAttribute
//{
//    public abstract string UnitTest { get; }
//}

//public sealed class UnitTestDiscoverer : ITraitDiscoverer
//{
//    public const string Key = nameof(UnitTestAttribute.UnitTest);

//    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
//    {
//        var category = traitAttribute.GetNamedArgument<string>(Key);
//        yield return new KeyValuePair<string, string>(Key, category);
//    }
//}
