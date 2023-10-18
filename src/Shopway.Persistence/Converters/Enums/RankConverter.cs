using Shopway.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.Enums;

public sealed class RankConverter : ValueConverter<Rank, string>
{
    public RankConverter() : base(rank => rank.ToString(), @string => (Rank)Enum.Parse(typeof(Rank), @string)) { }
}
