namespace CurrencyUpdaterService.Application.BackgroundServices;

public record UpdateJobOptions
{
    public int RecurringTimeInMinutes { get; init; }
}