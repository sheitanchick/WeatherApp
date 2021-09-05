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
                var weather = response.Data;

                if (weather != null)
                {
                    log.Data = new WeatherLogData
                    {
                        Provider = weather.Provider,
                        Latitude = weather.Latitude,
                        Longitude = weather.Longitude,
                        Description = weather.Description,
                        Temperature = weather.Temperature,
                        TemperatureFeelsLike = weather.TemperatureFeelsLike,
                        Pressure = weather.Pressure,
                        Humidity = weather.Humidity,
                        WindSpeed = weather.WindSpeed,
                        WindDirection = weather.WindDirection,
                        Cloudiness = weather.Cloudiness,
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
