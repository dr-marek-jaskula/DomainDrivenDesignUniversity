namespace Shopway.Presentation.Requests.Users;

public sealed record LoginRequest
(
    string Email,
    string Password
);