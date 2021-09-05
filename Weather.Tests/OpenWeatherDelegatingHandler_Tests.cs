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
    public class OpenWeatherDelegatingHandler_Tests
    {
        private readonly Mock<HttpMessageHandler> _innerHandlerMock;
        private readonly Mock<IOptions<OpenWeatherOptions>> _openWeatherOptionsMock;

        public OpenWeatherDelegatingHandler_Tests()
        {
            _openWeatherOptionsMock = new Mock<IOptions<OpenWeatherOptions>>();
            _innerHandlerMock = new Mock<HttpMessageHandler>();

            _openWeatherOptionsMock.SetupGet(z => z.Value)
                .Returns(new OpenWeatherOptions
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

            var _sut = new OpenWeatherDelegatingHandler(_openWeatherOptionsMock.Object)
            {
                InnerHandler = _innerHandlerMock.Object
            };

            var invoker = new HttpMessageInvoker(_sut);
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(_openWeatherOptionsMock.Object.Value.BaseUrl, ""));

            var result = await invoker.SendAsync(message, default);

            _innerHandlerMock
                .Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.Query.Contains($"appid={_openWeatherOptionsMock.Object.Value.ApiKey}")),
                    ItExpr.IsAny<CancellationToken>());

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
