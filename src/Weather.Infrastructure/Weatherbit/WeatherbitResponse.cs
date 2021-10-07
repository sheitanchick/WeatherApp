using System.Text.Json.Serialization;
using Weather.Domain.Interfaces;

namespace Weather.Infrastructure.Weatherbit
{
    public class WeatherbitResponse : IForecast
    {
        [JsonIgnore] public float Latitude => Data[0].Latitude;
        [JsonIgnore] public float Longitude => Data[0].Longitude;
        [JsonIgnore] public string Description => Data[0].Weather?.Description;
        [JsonIgnore] public float Temperature => Data[0].Temperature;
        [JsonIgnore] public float TemperatureFeelsLike => Data[0].TemperatureFeelsLike;
        [JsonIgnore] public float Pressure => Data[0].Pressure;
        [JsonIgnore] public float Humidity => Data[0].Humidity;
        [JsonIgnore] public float WindSpeed => Data[0].WindSpeed;
        [JsonIgnore] public float WindDirection => Data[0].WindDirection;
        [JsonIgnore] public float Cloudiness => Data[0].Cloudiness;
        [JsonIgnore] public string CountryCode => Data[0].CountryCode;
        [JsonIgnore] public string CityName => Data[0].CityName;

        [JsonPropertyName("data")] public WeatherbitData[] Data { get; set; } = new WeatherbitData[1] { new WeatherbitData() };
    }

    public class WeatherbitData
    {
        [JsonPropertyName("lon")] public float Longitude { get; set; }
        [JsonPropertyName("lat")] public float Latitude { get; set; }
        [JsonPropertyName("rh")] public float Humidity { get; set; }
        [JsonPropertyName("pres")] public float Pressure { get; set; }
        [JsonPropertyName("country_code")] public string CountryCode { get; set; }
        [JsonPropertyName("clouds")] public float Cloudiness { get; set; }
        [JsonPropertyName("city_name")] public string CityName { get; set; }
        [JsonPropertyName("wind_spd")] public float WindSpeed { get; set; }
        [JsonPropertyName("wind_dir")] public float WindDirection { get; set; }
        [JsonPropertyName("weather")] public WeatherInfo Weather { get; set; }
        [JsonPropertyName("temp")] public float Temperature { get; set; }
        [JsonPropertyName("app_temp")] public float TemperatureFeelsLike { get; set; }
    }

    public class WeatherInfo
    {
        [JsonPropertyName("description")] public string Description { get; set; }
    }
}