using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Weather.Application.Interfaces;
using Weather.Domain.Interfaces;
using Weather.Infrastructure.Utility;

namespace Weather.Application
{
    public class ForecastService : IForecastService
    {
        private readonly IEnumerable<IForecastProvider> _providers;

        public ForecastService(IEnumerable<IForecastProvider> providers)
        {
            _providers = providers;
        }

        public async Task<(IForecast Forecast, string Provider)> GetForecast(float lat, float lon, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, string>
            {
                ["lat"] = lat.ToString(),
                ["lon"] = lon.ToString()
            };

            var linkedTokenSource = ct.CreateLinkedTokenSource();

            var requests = _providers
                .ToDictionary(z => z.GetWeatherForecast(parameters, linkedTokenSource.Token), z => z.Type);

            var forecastTasks = requests.Keys.ToArray();

            var weather = await forecastTasks.GetFirstSuccessfullyExecutedTask();

            linkedTokenSource.Cancel();

            return (await weather, requests[weather]);
        }
    }
}