namespace Weather.WebApi.ViewModels
{
    public class WeatherResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
