using System.Reflection;

namespace Shopway.Tests.Integration;

[GenerateIntegrationTestTraits]
public static class AssemblyReference
{
    public static Assembly Assembly { get; set; } = typeof(AssemblyReference).Assembly;
}
