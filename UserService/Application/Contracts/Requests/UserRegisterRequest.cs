namespace UserService.Application.Contracts.Requests;

public sealed record UserRegisterRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Login { get; init; }
    public required string Password { get; init; }
}