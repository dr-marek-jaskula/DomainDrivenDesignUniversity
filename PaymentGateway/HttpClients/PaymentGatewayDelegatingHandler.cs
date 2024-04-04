namespace PaymentGateway.HttpClients;

public sealed class PaymentGatewayDelegatingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("api-version", "0.1");
        return base.SendAsync(request, cancellationToken);
    }
}