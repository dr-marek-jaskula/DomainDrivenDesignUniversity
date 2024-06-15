namespace Shopway.Application.Constants;

public static partial class Constants
{
    internal static class Page
    {
        internal static readonly int[] AllowedPageSizes = new[] { 5, 10, 15 };
        internal const string Then = nameof(Then);
        internal const string PageSize = nameof(PageSize);
        internal const string Cursor = nameof(Cursor);
    }
}
