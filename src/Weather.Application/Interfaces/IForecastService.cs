using System.Threading;
using System.Threading.Tasks;
using Weather.Domain.Interfaces;

namespace Weather.Application.Interfaces
{
    public interface IForecastService
    {
        Task<(IForecast Forecast, string Provider)> GetForecast(float lat, float lon, CancellationToken ct = default);
    }
}
