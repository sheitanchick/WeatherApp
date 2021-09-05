namespace Weather.DTO
{
    public class WeatherbitResponseDto
    {
        public WeatherbitDto[] data { get; set; } = new WeatherbitDto[0]; 
        public int count { get; set; }

        public WeatherDto ToWeatherDto(string provider)
        {
            if (data == null || data.Length == 0)
                return null;

            var dto = data[0];

            return new WeatherDto
            {
                Provider = provider,
                Latitude = dto.lat,
                Longitude = dto.lon,
                Description = dto.weather?.description,
                Temperature = dto.temp,
                TemperatureFeelsLike = dto.app_temp,
                Pressure = dto.pres,
                Humidity = dto.rh,
                WindSpeed = dto.wind_spd,
                WindDirection = dto.wind_dir,
                Cloudiness = dto.clouds,
                CountryCode = dto.country_code,
                CityName = dto.city_name
            };
        }
    }

    public class WeatherbitDto
    {
        public float rh { get; set; }
        public float lon { get; set; }
        public float pres { get; set; }
        public string country_code { get; set; }
        public float clouds { get; set; }
        public string city_name { get; set; }
        public float wind_spd { get; set; }
        public float wind_dir { get; set; }       
        public float vis { get; set; }
        public float precip { get; set; }
        public float lat { get; set; }
        public WeatherInfo weather { get; set; }
        public float temp { get; set; }
        public float app_temp { get; set; }
    }

    public class WeatherInfo
    {
        public string description { get; set; }
    }
}