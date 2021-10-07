using Moq;
using System;
using System.Threading.Tasks;
using Weather.Application;
using Weather.Application.Interfaces;
using Weather.Domain.Interfaces;
using Xunit;

namespace Weather.Tests.UnitTests
{
    public class ForecastLoggingService_Tests
    {
        private readonly Mock<IForecastService> _internalServiceMock;
        private readonly Mock<IWeatherLogger> _loggerMock;

        private readonly ForecastLoggingService _sut;

        public ForecastLoggingService_Tests()
        {
            _internalServiceMock = new Mock<IForecastService>();
            _loggerMock = new Mock<IWeatherLogger>();
            _sut = new ForecastLoggingService(_internalServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetForecast_SavesLogWhenInternalServiceSucceeeded()
        {
            _internalServiceMock
                .Setup(z => z.GetForecast(1, 2, default))
                .ReturnsAsync((It.IsAny<IForecast>(), nameof(_internalServiceMock)));

            var result = await _sut.GetForecast(1, 2);

            _loggerMock
                .Verify(z => z.Log(It.IsAny<IForecast>(), nameof(_internalServiceMock), It.Is<TimeSpan>(ts => ts.Ticks > 0), default), Times.Once);

            _loggerMock
                .Verify(z => z.LogError(It.IsAny<Exception>(), It.IsAny<TimeSpan>(), default), Times.Never);
        }

        [Fact]
        public async Task GetForecast_LogsErrorAndRethrowsTheExceptionWhenInternalServiceFailed()
        {
            var exception = new InvalidOperationException("Internal Service Failed");

            _internalServiceMock
                .Setup(z => z.GetForecast(1, 2, default))
                .ThrowsAsync(exception);

            Func<Task<(IForecast, string)>> function = () => _sut.GetForecast(1, 2);

            await Assert.ThrowsAsync<InvalidOperationException>(function);

            _loggerMock
                .Verify(z => z.LogError(exception, It.Is<TimeSpan>(ts => ts.Ticks > 0), default), Times.Once);

            _loggerMock
                .Verify(z => z.Log(It.IsAny<IForecast>(), It.IsAny<string>(), It.IsAny<TimeSpan>(), default), Times.Never);
        }
    }
}
