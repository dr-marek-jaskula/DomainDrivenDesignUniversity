using System.Numerics;

namespace Shopway.Domain.Utilities;

public static class NumberUtilities
{
    public static bool IsInRange<TNumber>(this TNumber input, TNumber lowerBound, TNumber upperBound)
        where TNumber : INumber<TNumber>
    {
        return input >= lowerBound && input <= upperBound;
    }

    public static bool NotInRange<TNumber>(this TNumber input, TNumber lowerBound, TNumber upperBound)
        where TNumber : INumber<TNumber>
    {
        return input.IsInRange(lowerBound, upperBound) is false;
    }
}