using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class Session : ValueObject
{
    public static readonly Error EmptyId = Error.New(
        $"{nameof(Session)}.{nameof(EmptyId)}",
        $"{nameof(EmptyId)} is empty.");

    public static readonly Error EmptySecret = Error.New(
        $"{nameof(Session)}.{nameof(EmptySecret)}",
        $"{nameof(EmptySecret)} is empty.");

    public static readonly Error EmptyPaymentIntentId = Error.New(
        $"{nameof(Session)}.{nameof(EmptyPaymentIntentId)}",
        $"{nameof(EmptyPaymentIntentId)} is empty.");

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

    public string Id { get; }
    public string Secret { get; }
    public string PaymentIntentId { get; }

    public static ValidationResult<Session> Create(string sessionId, string sessionSecret, string paymentIntentId)
    {
        var errors = Validate(sessionId, sessionSecret, paymentIntentId);
        return errors.CreateValidationResult(() => new Session(sessionId, sessionSecret, paymentIntentId));
    }

    public static IList<Error> Validate(string sessionId, string sessionSecret, string paymentIntentId)
    {
        return EmptyList<Error>()
            .If(sessionId.IsNullOrEmptyOrWhiteSpace(), EmptyId)
            .If(sessionSecret.IsNullOrEmptyOrWhiteSpace(), EmptySecret)
            .If(paymentIntentId.IsNullOrEmptyOrWhiteSpace(), EmptyPaymentIntentId);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Secret;
    }
}
