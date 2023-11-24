namespace Shopway.Domain.Common.Utilities;

public static class ArrayUtilities
{
    public static bool NotContains<TValue>(this TValue[] array, TValue value)
    {
        return array.Contains(value) is false;
    }
}