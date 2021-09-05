using System.Linq;

namespace Weather.DTO
{
    public class OpenWeatherResponseDto
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; } = new Weather[0];
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public Sys sys { get; set; }
        public string name { get; set; }

        public WeatherDto ToWeatherDto(string provider)
        {
            return new WeatherDto
            {
                Provider = provider,
                Latitude = coord.lat,
                Longitude = coord.lon,
                Description = weather.FirstOrDefault()?.description,
                Temperature = main.temp,
                TemperatureFeelsLike = main.feels_like,
                Pressure = main.pressure,
                Humidity = main.humidity,
                WindSpeed = wind.speed,
                WindDirection = wind.deg,
                Cloudiness = clouds.all,
                CountryCode = sys.country,
                CityName = name
            };
        }
    }

    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public float pressure { get; set; }
        public float humidity { get; set; }
        public float sea_level { get; set; }
        public float grnd_level { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public float deg { get; set; }
        public float gust { get; set; }
    }

    public class Clouds
    {
        public float all { get; set; }
    }

    public class Weather
    {
        public string description { get; set; }
    }

    public class Sys
    {
        public string country { get; set; }
    }
}