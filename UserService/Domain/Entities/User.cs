namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Login { get; init; }

    public string PasswordHash { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}
