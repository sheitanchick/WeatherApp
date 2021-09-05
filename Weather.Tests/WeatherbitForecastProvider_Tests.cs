using FluentAssertions;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Weather.DTO;
using Weather.Providers;
using Xunit;

namespace Weather.Tests
{
    public class WeatherbitForecastProvider_Tests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly Uri _baseUrl;
        public WeatherbitForecastProvider_Tests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _baseUrl = new Uri("https://0.0.0.0");
        }

        [Fact]
        public async Task GetWeatherForecast_ShouldReturnCorrectDataWhenRequestSucceded()
        {
            var data = new WeatherbitResponseDto
            {
                count = 1,
                data = new WeatherbitDto[1]
                {
                    new WeatherbitDto
                    {
                        temp = 10,
                        app_temp = 11,
                        wind_dir = 35
                    }
                }
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(data))
            };

            var expected = new WeatherDto
            {
                Provider = nameof(WeatherbitForecastProvider),
                Temperature = 10,
                TemperatureFeelsLike = 11,
                WindDirection = 35
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = _baseUrl };

            var _sut = new WeatherbitForecastProvider(httpClient);

            var dto = await _sut.GetWeatherForecast(new Dictionary<string, string>(), default);

            _handlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>());

            _handlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.PathAndQuery.StartsWith("/current")),
                    ItExpr.IsAny<CancellationToken>());

            dto.Should().NotBeNull();
            dto.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetWeatherForecast_ShouldThrowHttpRequestExceptionWhenSomeQueryParamsAreNotPassed()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            var queryRegex = new[]
            {
                new Regex(@"(lon=[\d.]+)"),
                new Regex(@"(lat=[\d.]+)")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => !queryRegex.All(r => r.IsMatch(req.RequestUri.Query))),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = _baseUrl };

            var _sut = new WeatherbitForecastProvider(httpClient);

            Func<Task> function = () => _sut.GetWeatherForecast(new Dictionary<string, string> 
            {
                ["lat"] = "12.0"
            }, default); 

            await function.Should().ThrowAsync<HttpRequestException>();            
        }
    }
}
