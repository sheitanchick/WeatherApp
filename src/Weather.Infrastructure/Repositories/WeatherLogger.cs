using System;
using System.Threading;
using System.Threading.Tasks;
using Weather.Domain.Entities;
using Weather.Domain.Interfaces;
using Weather.Infrastructure.Persistence;

namespace Weather.Infrastructure.Repositories
{
    public class WeatherLogger : IWeatherLogger
    {
        private readonly WeatherDbContext _dbContext;

        public WeatherLogger(WeatherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Log(IForecast forecast, string provider, TimeSpan elapsed, CancellationToken ct = default)
        {
            var log = new WeatherLog
            {
                Elapsed = elapsed,
                Data = new WeatherLogData
                {
                    Provider = provider,
                    Latitude = forecast.Latitude,
                    Longitude = forecast.Longitude,
                    Description = forecast.Description,
                    Temperature = forecast.Temperature,
                    TemperatureFeelsLike = forecast.TemperatureFeelsLike,
                    Pressure = forecast.Pressure,
                    Humidity = forecast.Humidity,
                    WindSpeed = forecast.WindSpeed,
                    WindDirection = forecast.WindDirection,
                    Cloudiness = forecast.Cloudiness,
                    CountryCode = forecast.CountryCode,
                    CityName = forecast.CityName
                }
            };

            _dbContext.Add(log);
            
            await Task.Delay(100);

            await _dbContext.SaveChangesAsync(ct);
        }

        public Task LogError(Exception exception, TimeSpan elapsed, CancellationToken ct = default)
        {
            var log = new WeatherLog
            {
                Elapsed = elapsed,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            };

            _dbContext.Add(log);

            return _dbContext.SaveChangesAsync(ct);
        }
    }
}
