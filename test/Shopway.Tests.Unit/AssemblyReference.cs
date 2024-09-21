using System.Reflection;

namespace Shopway.Tests.Unit;

[GenerateTraits]
public static class AssemblyReference
{
    public static Assembly Assembly { get; set; } = typeof(AssemblyReference).Assembly;
}
