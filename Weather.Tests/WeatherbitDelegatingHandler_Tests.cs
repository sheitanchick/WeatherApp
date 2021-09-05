using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Weather.Configuration;
using Weather.Configuration.Extensions;
using Xunit;

namespace Weather.Tests
{
    public class WeatherbitDelegatingHandler_Tests
    {
        private readonly Mock<HttpMessageHandler> _innerHandlerMock;
        private readonly Mock<IOptions<WeatherbitOptions>> _weatherbitOptionsMock;

        public WeatherbitDelegatingHandler_Tests()
        {
            _weatherbitOptionsMock = new Mock<IOptions<WeatherbitOptions>>();
            _innerHandlerMock = new Mock<HttpMessageHandler>();

            _weatherbitOptionsMock.SetupGet(z => z.Value)
                .Returns(new WeatherbitOptions
                {
                    BaseUrl = new Uri("https://0.0.0.0/"),
                    ApiKey = "test_key"
                });
        }

        [Fact]
        public async Task SendAsync_AddsApiKeyToQuery()
        {
            _innerHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var _sut = new WeatherbitDelegatingHandler(_weatherbitOptionsMock.Object)
            {
                InnerHandler = _innerHandlerMock.Object
            };

            var invoker = new HttpMessageInvoker(_sut);
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(_weatherbitOptionsMock.Object.Value.BaseUrl, ""));

            var result = await invoker.SendAsync(message, default);

            _innerHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.Query.Contains($"key={_weatherbitOptionsMock.Object.Value.ApiKey}")),
                    ItExpr.IsAny<CancellationToken>());

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
