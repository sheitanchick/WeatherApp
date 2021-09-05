namespace Weather.DTO
{
    public class WeatherResponse<T>
    {
        public bool Success { get; set; }
        public T Weather { get; set; }
    }
}
