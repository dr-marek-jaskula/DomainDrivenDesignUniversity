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

    public static bool NotNullOrEmptyString(this object? value)
    {
        return IsNullOrEmptyString(value) is false;
    }

    public static bool IsNullOrEmptyObject(this object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is System.DBNull)
        {
            return true;
        }

        return false;
    }

    public static bool NotNullOrEmptyObject(this object? value)
    {
        return IsNullOrEmptyObject(value) is false;
    }
}
