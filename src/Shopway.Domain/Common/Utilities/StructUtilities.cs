namespace Shopway.Domain.Common.Utilities;

public static class StructUtilities
{
    public static bool IsOneOf<TValue>(this TValue value, IEnumerable<TValue> values)
        where TValue : struct, IEquatable<TValue>
    {
        foreach (var item in values)
        {
            if (value.Equals(item))
            {
                return true;
            }
        }

        return false;
    }
}
