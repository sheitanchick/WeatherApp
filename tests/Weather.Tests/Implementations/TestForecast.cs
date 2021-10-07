using Weather.Domain.Interfaces;

namespace Weather.Tests.Implementations
{
    internal class TestForecast : IForecast
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
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
