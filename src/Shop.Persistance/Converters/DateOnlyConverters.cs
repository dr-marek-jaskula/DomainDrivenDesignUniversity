using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters;

//Used for conversion from DateTime to DateOnly and backwards.
//The example of use is done in PersonConfiguration

public sealed class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue), dateTime => DateOnly.FromDateTime(dateTime))
    {
    }
}

public sealed class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base((d1, d2) => d1.DayNumber == d2.DayNumber, d => d.GetHashCode())
    {
    }
}