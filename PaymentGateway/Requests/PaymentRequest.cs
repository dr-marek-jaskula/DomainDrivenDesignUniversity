namespace PaymentGateway.Requests;

public sealed record PaymentRequest(List<string> Items, int Total, string Currency);
