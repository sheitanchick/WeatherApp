using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Threading.Tasks;
using Weather.DTO;
using Weather.Models;
using Weather.Repositories;

namespace Weather.Utility
{
    public class WeatherLoggingActionFilter : IAsyncActionFilter
    {
        private readonly IWeatherLogRepository _logRepository;

        public WeatherLoggingActionFilter(IWeatherLogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await next();

            stopwatch.Stop();

            var log = new WeatherLog
            {
                Elapsed = stopwatch.Elapsed,
                Message = result.Exception?.Message,
                StackTrace = result.Exception?.StackTrace
            };

            _logRepository.Add(log);

            if (TryExtractWeatherResponse(result.Result, out WeatherResponse<WeatherDto> response))
            {
                var weather = response.Weather;

                if (weather != null)
                {
                    log.Data = new WeatherLogData
                    {
                        Provider = weather.Provider,
                        Latitude = weather.Latitude.GetValueOrDefault(),
                        Longitude = weather.Longitude.GetValueOrDefault(),
                        Description = weather.Description,
                        Temperature = weather.Temperature.GetValueOrDefault(),
                        TemperatureFeelsLike = weather.TemperatureFeelsLike.GetValueOrDefault(),
                        Pressure = weather.Pressure.GetValueOrDefault(),
                        Humidity = weather.Humidity.GetValueOrDefault(),
                        WindSpeed = weather.WindSpeed.GetValueOrDefault(),
                        WindDirection = weather.WindDirection.GetValueOrDefault(),
                        Cloudiness = weather.Cloudiness.GetValueOrDefault(),
                        CountryCode = weather.CountryCode,
                        CityName = weather.CityName
                    };
                }
            }

            await _logRepository.SaveChangesAsync();
        }

        private bool TryExtractWeatherResponse(IActionResult actionResult, out WeatherResponse<WeatherDto> response)
        {
            response = null;
            var success = false;

            if (actionResult is ObjectResult objectResult 
                && objectResult.Value is WeatherResponse<WeatherDto> weatherResponse  
                && weatherResponse != null)
            {
                response = weatherResponse;
                success = true;
            }

            return success;
        }
    }
}
