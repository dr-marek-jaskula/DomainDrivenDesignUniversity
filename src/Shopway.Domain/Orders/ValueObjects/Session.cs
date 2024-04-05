using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Orders.Errors.DomainErrors;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class Session : ValueObject
{
    public string Id { get; }
    public string Secret { get; }
    public string PaymentIntentId { get; }

    private Session(string sessionId, string sessionSecret, string paymentIntentId)
    {
        Id = sessionId;
        Secret = sessionSecret;
        PaymentIntentId = paymentIntentId;
    }

    //Empty constructor in this case is required by EF Core, because has a complex type as a parameter in the default constructor.
    private Session()
    {
    }

    public static ValidationResult<Session> Create(string sessionId, string sessionSecret, string paymentIntentId)
    {
        var errors = Validate(sessionId, sessionSecret, paymentIntentId);
        return errors.CreateValidationResult(() => new Session(sessionId, sessionSecret, paymentIntentId));
    }

    public static IList<Error> Validate(string sessionId, string sessionSecret, string paymentIntentId)
    {
        return EmptyList<Error>()
            .If(sessionId.IsNullOrEmptyOrWhiteSpace(), SessionError.EmptyId)
            .If(sessionSecret.IsNullOrEmptyOrWhiteSpace(), SessionError.EmptySecret)
            .If(paymentIntentId.IsNullOrEmptyOrWhiteSpace(), SessionError.EmptyPaymentIntentId);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Secret;
    }
}
