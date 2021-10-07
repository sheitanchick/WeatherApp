using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Weather.Domain.Interfaces
{
    public interface IForecastProvider
    {
        Task<IForecast> GetWeatherForecast(IDictionary<string, string> parameters, CancellationToken ct);
        string Type { get; }
    }
}
