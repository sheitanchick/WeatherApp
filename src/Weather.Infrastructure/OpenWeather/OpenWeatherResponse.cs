using System.Text.Json.Serialization;
using Weather.Domain.Interfaces;

namespace Weather.Infrastructure.OpenWeather
{
    public class OpenWeatherResponse : IForecast
    {
        [JsonIgnore] public float Latitude => Coord.Latitude;
        [JsonIgnore] public float Longitude => Coord.Longitude;
        [JsonIgnore] public string Description => Weather[0].Description;
        [JsonIgnore] public float Temperature => Main.Temperature;
        [JsonIgnore] public float TemperatureFeelsLike => Main.TemperatureFeelsLike;
        [JsonIgnore] public float Pressure => Main.Pressure;
        [JsonIgnore] public float Humidity => Main.Humidity;
        [JsonIgnore] public float WindSpeed => Wind.Speed;
        [JsonIgnore] public float WindDirection => Wind.Direction;
        [JsonIgnore] public float Cloudiness => Clouds.Cloudiness;
        [JsonIgnore] public string CountryCode => Sys.Country;
        [JsonIgnore] public string CityName => Name;

        [JsonPropertyName("coord")] public Coord Coord { get; set; }
        [JsonPropertyName("weather")] public Weather[] Weather { get; set; } = new Weather[1] { new Weather() };
        [JsonPropertyName("main")] public Main Main { get; set; }
        [JsonPropertyName("wind")] public Wind Wind { get; set; }
        [JsonPropertyName("clouds")] public Clouds Clouds { get; set; }
        [JsonPropertyName("sys")] public Sys Sys { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }

    public class Coord
    {
        [JsonPropertyName("lon")] public float Longitude { get; set; }
        [JsonPropertyName("lat")] public float Latitude { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("temp")] public float Temperature { get; set; }
        [JsonPropertyName("feels_like")] public float TemperatureFeelsLike { get; set; }
        [JsonPropertyName("pressure")] public float Pressure { get; set; }
        [JsonPropertyName("humidity")] public float Humidity { get; set; }
    }

    public class Wind
    {
        [JsonPropertyName("speed")] public float Speed { get; set; }
        [JsonPropertyName("deg")] public float Direction { get; set; }
    }

    public class Clouds
    {
        [JsonPropertyName("all")] public float Cloudiness { get; set; }
    }

    public class Weather
    {
        [JsonPropertyName("description")] public string Description { get; set; }
    }

    public class Sys
    {
        [JsonPropertyName("country")] public string Country { get; set; }
    }
}