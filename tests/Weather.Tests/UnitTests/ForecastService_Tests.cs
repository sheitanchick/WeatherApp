using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weather.Application;
using Weather.Domain.Interfaces;
using Weather.Tests.Implementations;
using Xunit;

namespace Weather.Tests.UnitTests
{
    public class ForecastService_Tests
    {
        private readonly ForecastService _sut;
        private readonly Mock<IForecastProvider> _fastForecastProviderMock;
        private readonly Mock<IForecastProvider> _slowForecastProviderMock;

        public ForecastService_Tests()
        {
            _fastForecastProviderMock = new Mock<IForecastProvider>();
            _slowForecastProviderMock = new Mock<IForecastProvider>();
            
            _sut = new ForecastService(new[] 
            {
                _fastForecastProviderMock.Object,
                _slowForecastProviderMock.Object
            });
        }

        [Fact]
        public async Task GetForecast_ReturnsForecastWhenUnderlyingProviderSucceeded()
        {
            var forecast = new TestForecast 
            {
                Temperature = 15.5F,
                Description = "TestForecastDescription"
            };

            _fastForecastProviderMock
                .Setup(z => z.Type)
                .Returns(nameof(_fastForecastProviderMock));

            _fastForecastProviderMock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(forecast);

            var result = await _sut.GetForecast(10, 10);

            Assert.Equal(nameof(_fastForecastProviderMock), result.Provider);
            Assert.Equal(forecast.Temperature, result.Forecast.Temperature);
            Assert.Equal(forecast.Description, result.Forecast.Description);
        }

        [Fact]
        public async Task GetForecast_ReturnsFastestForecastWhenBothUnderlyingProvidersSucceeded()
        {
            var forecast = new TestForecast
            {
                Temperature = 15.5F,
                Description = "TestForecastDescription"
            };

            _fastForecastProviderMock
                .Setup(z => z.Type)
                .Returns(nameof(_fastForecastProviderMock));

            _fastForecastProviderMock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.Run<IForecast>(async () => 
                {
                    await Task.Delay(1);
                    return forecast;
                }));

            _slowForecastProviderMock
                .Setup(z => z.Type)
                .Returns(nameof(_slowForecastProviderMock));

            _slowForecastProviderMock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.Run<IForecast>(async () =>
                {
                    await Task.Delay(20);  
                    return null;
                }));

            var result = await _sut.GetForecast(10, 10);

            Assert.Equal(nameof(_fastForecastProviderMock), result.Provider);
            Assert.Equal(forecast.Temperature, result.Forecast.Temperature);
            Assert.Equal(forecast.Description, result.Forecast.Description);
        }

        [Fact]
        public async Task GetForecast_ThrowsExceptionWhenBothUnderlyingProvidersFailed()
        {
            _fastForecastProviderMock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            _slowForecastProviderMock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            Func<Task> result = () => _sut.GetForecast(10, 10);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(result);
            Assert.Equal("All tasks have failed!", exception.Message);
        }
    }
}
