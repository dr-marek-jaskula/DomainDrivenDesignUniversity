namespace PaymentGateway.Requests;

public sealed record ConfigureIssuerRequest(string Issuer, string Secret);
