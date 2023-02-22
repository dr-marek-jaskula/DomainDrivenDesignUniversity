using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters;

//Used for conversion from DateTimeOffset to DateOnly and backwards.
//The example of use is done in PersonConfiguration
public sealed class DateOnlyConverter : ValueConverter<DateOnly, DateTimeOffset>
{
    public DateOnlyConverter() : base(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue), dateTime => DateOnly.FromDateTime(dateTime.DateTime))
    {
    }
}

public sealed class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base((d1, d2) => d1.DayNumber == d2.DayNumber, d => d.GetHashCode())
    {
    }
}