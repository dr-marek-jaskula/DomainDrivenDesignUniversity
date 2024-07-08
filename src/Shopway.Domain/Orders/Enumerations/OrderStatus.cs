using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Orders.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    [EnumMember(Value = nameof(New))]
    New = 1,
    [EnumMember(Value = nameof(InProgress))]
    InProgress,
    [EnumMember(Value = nameof(Shipped))]
    Shipped,
    [EnumMember(Value = nameof(Delivered))]
    Delivered,
    [EnumMember(Value = nameof(OnHold))]
    OnHold,
    [EnumMember(Value = nameof(Rejected))]
    Rejected
}
