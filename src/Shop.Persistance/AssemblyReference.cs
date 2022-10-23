using System.Reflection;

namespace Shopway.Persistence;

//This class is used to refer the this project, to scan for configurations
public sealed class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}