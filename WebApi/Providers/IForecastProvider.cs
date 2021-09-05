using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weather.DTO;

namespace Weather.Providers
{
    public interface IForecastProvider
    {
        Task<WeatherDto> GetWeatherForecast(IDictionary<string, string> parameters, CancellationToken ct);
        string Type { get; }
    }
}
