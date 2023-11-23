using Shopway.Domain.Enums;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Abstractions;
using static Shopway.Domain.Enums.PaymentStatus;

namespace Shopway.Domain.Entities;

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

    public void PaymentReceived()
    {
        Status = Received;
    }
}