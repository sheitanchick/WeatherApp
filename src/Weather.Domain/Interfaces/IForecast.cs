namespace Weather.Domain.Interfaces
{
    public interface IForecast
    {
        float Latitude { get; }
        float Longitude { get; }
        string Description { get; }
        float Temperature { get; }
        float TemperatureFeelsLike { get; }
        float Pressure { get; }
        float Humidity { get; }
        float WindSpeed { get; }
        float WindDirection { get; }
        float Cloudiness { get; }
        string CountryCode { get; }
        string CityName { get; }
    }
}
