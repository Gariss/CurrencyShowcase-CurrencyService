namespace UserService.Application.Contracts.Requests;

public sealed record LoginRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; }
};