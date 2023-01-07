namespace Shopway.Presentation.Requests.Users;

public sealed record RegisterRequest
(
    string Username,
    string Email
);