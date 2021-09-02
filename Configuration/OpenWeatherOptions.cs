using System;

namespace Weather.Configuration
{
    public class OpenWeatherOptions
    {
        public Uri BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
