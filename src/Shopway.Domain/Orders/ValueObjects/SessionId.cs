using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Orders.Errors.DomainErrors;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class SessionId : ValueObject
{
    public new string Value { get; }

    private SessionId(string value)
    {
        Value = value;
    }

    public static ValidationResult<SessionId> Create(string sessionId)
    {
        var errors = Validate(sessionId);
        return errors.CreateValidationResult(() => new SessionId(sessionId));
    }

    public static IList<Error> Validate(string sessionId)
    {
        return EmptyList<Error>()
            .If(sessionId.IsNullOrEmptyOrWhiteSpace(), SessionIdError.Empty);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
