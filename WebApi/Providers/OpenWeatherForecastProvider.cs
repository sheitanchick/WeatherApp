using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Weather.DTO;

namespace Weather.Providers
{
    public class OpenWeatherForecastProvider : IForecastProvider
    {
        private readonly HttpClient _httpClient;

        public OpenWeatherForecastProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string Type => nameof(OpenWeatherForecastProvider);

        public async Task<WeatherDto> GetWeatherForecast(IDictionary<string, string> parameters, CancellationToken ct = default)
        {
            parameters.TryAdd("units", "metric");

            var pathAndQuery = QueryHelpers.AddQueryString("weather", parameters);
            
            var response = await _httpClient.GetAsync(pathAndQuery, ct);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<OpenWeatherResponseDto>(content);

            return data.ToWeatherDto(Type);
        }
    }
}
