namespace UserService.Application.Contracts.Responses;

public sealed record LoginResponse
{
    public string Token { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}
