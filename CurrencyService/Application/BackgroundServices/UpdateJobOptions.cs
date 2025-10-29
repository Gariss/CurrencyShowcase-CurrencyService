namespace CurrencyService.Application.BackgroundServices;

public record UpdateJobOptions
{
    public int RecurringTimeInMinutes { get; init; }
}