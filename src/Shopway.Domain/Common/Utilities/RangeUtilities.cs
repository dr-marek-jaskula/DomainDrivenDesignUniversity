namespace Shopway.Domain.Common.Utilities;

public static class RangeUtility
{
    public static RangeEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }
}

public sealed record RangeEnumerator
{
    private int _current;
    private readonly int _end;

    public RangeEnumerator(Range range)
    {
        if (range.End.IsFromEnd)
        {
            throw new NotSupportedException("Undefined enumeration is not supported");
        }

        _current = range.Start.Value - 1;
        _end = range.End.Value;
    }

    public int Current => _current;

    public bool MoveNext()
    {
        _current++;
        return _current <= _end;
    }
}
