using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.Orders.Enumerations;
using Shopway.Domain.Orders.ValueObjects;
using static Shopway.Domain.Orders.Enumerations.PaymentStatus;

namespace Shopway.Domain.Orders;

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

    public Result Refund()
    {
        if (Status is not Received)
        {
            return Result.Failure(Error.InvalidOperation("Refund cannot be performed on a not received payment."));
        }

        IsRefunded = true;
        return Result.Success();
    }

    public void SetStatus(PaymentStatus paymentStatus)
    {
        Status = paymentStatus;
    }

    public void SetSession(Session session)
    {
        Session = session;
    }
}