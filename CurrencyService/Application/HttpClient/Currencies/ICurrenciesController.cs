using Refit;

namespace CurrencyService.Application.HttpClient.Currencies;

public interface ICurrenciesController
{
    /// <summary>
    /// Get Curencies List
    /// </summary>
    [Get("")]
    Task<IApiResponse<ValCurs>> GetAsync(CancellationToken cancellationToken);
}