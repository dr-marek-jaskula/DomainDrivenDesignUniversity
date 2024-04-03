namespace Shopway.Infrastructure.Payments.DummyGatewayTypes;

public class SessionService
{
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