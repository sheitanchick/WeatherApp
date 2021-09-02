using System;

namespace Weather.DTO
{
    public class WeatherbitResponseDto
    {
        public WeatherbitDto[] data { get; set; }
        public int count { get; set; }
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