using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Weather.Application.Interfaces;
using Weather.Domain.Interfaces;

namespace Weather.Application
{
    public class ForecastLoggingService : IForecastService
    {
        private readonly IForecastService _internalService;
        private readonly IWeatherLogger _logger;

        public ForecastLoggingService(IForecastService internalService, IWeatherLogger logger)
        {
            _internalService = internalService;
            _logger = logger;
        }

        public async Task<(IForecast Forecast, string Provider)> GetForecast(float lat, float lon, CancellationToken ct = default)
        {
            var stopwatch = Stopwatch.StartNew();

            try 
            {
                var result = await _internalService.GetForecast(lat, lon, ct);

                await _logger.Log(result.Forecast, result.Provider, stopwatch.Elapsed, ct);
                
                return result;
            }
            catch (Exception e)
            {
                await _logger.LogError(e, stopwatch.Elapsed, ct);
                
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }
        }
    }
}
