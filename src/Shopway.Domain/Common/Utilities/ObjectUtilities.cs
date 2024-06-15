namespace Shopway.Domain.Common.Utilities;

public static class ObjectUtilities
{
    public static bool IsNullOrEmptyString(this object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is string @string && @string == string.Empty)
        {
            return true;
        }

        return false;
    }
}
