﻿namespace Shopway.Infrastructure.Payments.DummyGatewayTypes.Sessions;

public class SessionLineItemPriceDataOptions
{
    public string Currency { get; set; } = string.Empty;
    public SessionLineItemPriceDataProductDataOptions? ProductData { get; set; }
    public decimal UnitAmountDecimal { get; set; } = decimal.Zero;
}
