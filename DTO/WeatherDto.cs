using System;

namespace Weather.DTO
{
    public class WeatherDto
    {
        public string Description { get; set; }
        public float Temperature { get; set; }
        public float TemperatureFeelsLike { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public float WindSpeed { get; set; }
        public float WindDirection { get; set; }
        public float Cloudiness { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
    }
}
