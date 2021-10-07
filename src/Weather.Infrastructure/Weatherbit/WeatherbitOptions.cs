using System;

namespace Weather.Infrastructure.Weatherbit
{
    public class WeatherbitOptions
    {
        public Uri BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
