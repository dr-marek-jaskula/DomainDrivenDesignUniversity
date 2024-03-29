using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class SessionIdConverter : ValueConverter<SessionId, string>
{
    public SessionIdConverter() : base(sessionId => sessionId.Value, @string => SessionId.Create(@string).Value) { }
}

public sealed class SessionIdComparer : ValueComparer<SessionId>
{
    public SessionIdComparer() : base((sessionId1, sessionId2) => sessionId1!.Value == sessionId2!.Value, sessionId => sessionId.GetHashCode()) { }
}