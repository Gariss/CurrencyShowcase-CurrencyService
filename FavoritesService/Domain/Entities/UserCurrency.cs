namespace FavoritesService.Domain.Entities;

public class UserCurrency
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string CurrencyId { get; init; }
    public Currency Currency { get; init; }
}
