using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Converters.Enums;

public sealed class RankConverter : ValueConverter<Rank, string>
{
    public RankConverter() : base(rank => rank.ToString(), @string => (Rank)Enum.Parse(typeof(Rank), @string)) { }
}
