﻿using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Weather.DTO;

namespace Weather.Providers
{
    public class WeatherbitForecastProvider : IForecastProvider
    {
        private readonly HttpClient _httpClient;

        public WeatherbitForecastProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string Type => nameof(WeatherbitForecastProvider);

        public async Task<WeatherDto> GetWeatherForecast(IDictionary<string, string> parameters, CancellationToken ct = default)
        {
            var pathAndQuery = QueryHelpers.AddQueryString("current", parameters);

            var response = await _httpClient.GetAsync(pathAndQuery, ct);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var dto = JsonSerializer.Deserialize<WeatherbitResponseDto>(content);

            return dto.ToWeatherDto(Type);
        }
    }
}