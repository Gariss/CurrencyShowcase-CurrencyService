namespace CurrencyService.Domain.Entities;

public class Currency
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required decimal Rate { get; set; }
}
