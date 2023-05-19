namespace Shopway.Domain.Utilities;

public static class EnumUtilities
{
    public static IEnumerable<string> GetEnumNames<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetNames(typeof(TEnum));
    }

    public static IEnumerable<string> GetEnumNames<TType>(this TType type)
        where TType : Enum
    {
        return Enum.GetNames(type.GetType());
    }
}