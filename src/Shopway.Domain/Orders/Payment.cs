﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Orders.Enumerations;
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