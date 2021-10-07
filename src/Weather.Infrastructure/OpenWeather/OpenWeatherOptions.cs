using System;

namespace Weather.Infrastructure.OpenWeather
{
    public class OpenWeatherOptions
    {
        public Uri BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
