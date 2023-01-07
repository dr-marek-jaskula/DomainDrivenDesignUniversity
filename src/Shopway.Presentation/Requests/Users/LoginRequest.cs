namespace Shopway.Presentation.Requests.Users;

public sealed record LoginRequest
(
    string Username,
    string Email
);