namespace Shopway.Infrastructure.Payments.DummyGatewayTypes;

[Serializable]
public class PaymentGatewayException : Exception
{
    public PaymentGatewayException()
    {
    }

    public PaymentGatewayException(string? message) : base(message)
    {
    }

    public PaymentGatewayException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
