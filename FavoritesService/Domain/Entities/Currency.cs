namespace FavoritesService.Domain.Entities;

public class Currency
{
    public string Id { get; init; }
    public string CharCode { get; init; }
    public string Name { get; init; }
    public decimal Rate { get; init; }
}
