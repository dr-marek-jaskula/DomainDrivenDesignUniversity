namespace Shopway.Infrastructure.Payments.DummyGatewayTypes;

public class SessionService
{
    /// <summary>
    /// This send a request to the Payment Gateway to create a session for a customer. This session will be available on a given url.
    /// For demo purposes, there is no need to create a session dummy Payment Gateway side.
    /// </summary>
    internal async Task<Session> CreateAsync(SessionCreateOptions options)
    {
        return new Session()
        {
            ClientSecret = Guid.NewGuid().ToString(),
            Id = Guid.NewGuid().ToString(),
            Url = "http://localhost:61045/redirect-to-payment-session"
        };
    }
}