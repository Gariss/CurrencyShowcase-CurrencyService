using Refit;

namespace CurrencyUpdaterService.Application.HttpClient.Currencies;

public interface ICurrenciesController
{
    /// <summary>
    /// Get Curencies List
    /// </summary>
    [Get("")]
    Task<IApiResponse<ValCurs>> GetAsync(CancellationToken cancellationToken);
}