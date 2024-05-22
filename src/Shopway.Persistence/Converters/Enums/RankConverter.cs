using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Users.Enumerations;

namespace Shopway.Persistence.Converters.Enums;

//NOTE: LEFT FOR TUTORIAL PURPOSES. Other Enum converters are source generated.

public sealed class RankConverter : ValueConverter<Rank, string>
{
    public RankConverter() : base(rank => rank.ToString(), @string => (Rank)Enum.Parse(typeof(Rank), @string)) { }
}
