using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Controllers;
using Weather.DTO;
using Weather.Providers;
using Xunit;

namespace Weather.Tests
{
    public class ForecastController_Tests
    {
        protected readonly ForecastController _sut;
        protected readonly Mock<IForecastProvider> _provider1Mock;
        protected readonly Mock<IForecastProvider> _provider2Mock;

        public ForecastController_Tests()
        {
            _provider1Mock = new Mock<IForecastProvider>();
            _provider2Mock = new Mock<IForecastProvider>();

            var providers = new[] { _provider1Mock.Object, _provider2Mock.Object };

            _sut = new ForecastController(providers) 
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() 
                }
            };
        }

        [Fact]
        public async Task Index_ShouldReturnFirstResult() 
        {
            var slower = new WeatherDto
            {
                Description = "slower"
            };

            var faster = new WeatherDto()
            {
                Description = "faster"
            };

            _provider1Mock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), default))
                .Returns(Task.Run(async () =>
                {
                    await Task.Delay(100);
                    return slower;
                }));

            _provider2Mock
               .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), default))
               .Returns(Task.Run(() =>
               {
                   return faster;
               }));

            var result = await _sut.Index(0, 0);

            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(faster); 
        }

        [Fact]
        public async Task Index_ShouldPassCorrectQueryParams()
        {
            _provider1Mock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), default))
                .Returns(Task.Run(() =>
                {
                    return new WeatherDto();
                }));

            var lat = 12.3F;
            var lon = 52;

            var result = await _sut.Index(lat, lon);

            _provider1Mock
                .Verify(z => z.GetWeatherForecast(
                    It.Is<IDictionary<string, string>>(z => z["lat"] == $"{lat}" && z["lon"] == $"{lon}"), default), Times.Once);
        }

        [Fact]
        public async Task Index_ShouldThrowExceptionWhenBothProviderFailed()
        {
            _provider1Mock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), default))
                .Returns(Task.Run(() =>
                {
                    return Task.FromException<WeatherDto>(new Exception());
                }));

            _provider2Mock
               .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), default))
               .Returns(Task.Run(() =>
               {
                   return Task.FromException<WeatherDto>(new Exception());
               }));

            Func<Task<WeatherResponse<WeatherDto>>> function = async () => await _sut.Index(0, 0);

            await function.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Index_ShouldReturnSlowerResultWhenFastestProviderFailed()
        {
            var slower = new WeatherDto
            {
                Description = "slower"
            };

            _provider1Mock
                .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), default))
                .Returns(Task.Run(() =>
                {
                    return Task.FromException<WeatherDto>(new Exception());
                }));

            _provider2Mock
               .Setup(z => z.GetWeatherForecast(It.IsAny<IDictionary<string, string>>(), default))
               .Returns(Task.Run(async () =>
               {
                   await Task.Delay(50);
                   return slower;
               }));

            var result = await _sut.Index(0, 0);

            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(slower);
        }
    }
}
