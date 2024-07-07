using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    [EnumMember(Value = nameof(Ascending))]
    Ascending,
    [EnumMember(Value = nameof(Descending))]
    Descending
}
