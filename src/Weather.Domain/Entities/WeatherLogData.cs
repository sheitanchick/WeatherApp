namespace Weather.Domain.Entities
{
    public class WeatherLogData
    {
        public long WeatherLogId { get; set; }

        public string Provider { get; set; }
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

        public WeatherLog WeatherLog { get; set; }
    }
}
