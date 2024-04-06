namespace PaymentGateway.Requests;

public sealed record PublishRequest(string Issuer, string SessionId, string ClientSecret);