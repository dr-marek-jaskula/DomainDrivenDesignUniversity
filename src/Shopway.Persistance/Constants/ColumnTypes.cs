namespace Shopway.Persistence.Constants;

internal static class ColumnTypes
{
    internal const string UniqueIdentifier = nameof(UniqueIdentifier);
    internal const string TinyInt = nameof(TinyInt);
    internal static string DateTimeOffset(int lenght) => $"{nameof(DateTimeOffset)}({lenght})";
    internal static string NChar(int lenght) => $"{nameof(NChar)}({lenght})";
    internal static string VarChar(int lenght) => $"{nameof(VarChar)}({lenght})";
}