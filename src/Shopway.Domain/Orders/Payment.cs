using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Orders.Enumerations;
using Shopway.Domain.Orders.ValueObjects;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Orders.Enumerations.PaymentStatus;

namespace Shopway.Domain.Orders;

[GenerateEntityId]
public sealed class Payment : Entity<PaymentId>, IAuditable
{
    private Payment
    (
        PaymentId id,
        PaymentStatus status
    )
        : base(id)
    {
        Status = status;
    }

    // Empty constructor in this case is required by EF Core
    private Payment()
    {
    }

    public Session? Session { get; private set; }
    public bool IsRefunded { get; private set; } = false;
    public OrderHeaderId OrderHeaderId { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public static Payment Create()
    {
        return new Payment
        (
            id: PaymentId.New(),
            status: NotReceived
        );
    }

    internal async Task<Result> Refund(IPaymentGatewayService paymentGatewayService)
    {
        var errors = EmptyList<Error>()
            .If(IsRefunded, Error.InvalidOperation("Payment is already refunded"))
            .If(Status is not Received, Error.InvalidOperation("Refund cannot be performed when payment was not received"))
            .If(Session is null || Session.PaymentIntentId.IsNullOrEmptyOrWhiteSpace(), Error.NullReference("Session or PaymentIntentId is not set"));

        if (errors.NotNullOrEmpty())
        {
            return ValidationResult.WithErrors(errors);
        }

        var refundResult = await paymentGatewayService.Refund(Session!);

        if (refundResult.IsFailure)
        {
            return refundResult;
        }

        IsRefunded = true;
        return Result.Success();
    }

    internal void SetStatus(PaymentStatus paymentStatus)
    {
        Status = paymentStatus;
    }

    public void SetSession(Session session)
    {
        Session = session;
    }
}
