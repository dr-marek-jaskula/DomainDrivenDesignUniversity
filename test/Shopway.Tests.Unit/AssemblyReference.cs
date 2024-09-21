using System.Reflection;

namespace Shopway.Tests.Unit;

[GenerateUnitTestTraits]
public static class AssemblyReference
{
    public static Assembly Assembly { get; set; } = typeof(AssemblyReference).Assembly;
}
