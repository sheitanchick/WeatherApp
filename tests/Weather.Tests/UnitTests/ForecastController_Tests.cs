using AutoFixture;
using AutoFixture.Kernel;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Weather.Application.Interfaces;
using Weather.Domain.Interfaces;
using Weather.Tests.Implementations;
using Weather.WebApi;
using Weather.WebApi.Controllers;
using Weather.WebApi.ViewModels;
using Xunit;

namespace Weather.Tests.UnitTests
{
    public class ForecastController_Tests
    {
        protected readonly ForecastController _sut;
        protected readonly Mock<IForecastService>  _serviceMock;

        public ForecastController_Tests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            var mapper = config.CreateMapper();

            _serviceMock = new Mock<IForecastService>();

            _sut = new ForecastController(_serviceMock.Object, mapper) 
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() 
                }
            };
        }

        [Fact]
        public async Task Index_ReturnsProperResponse()
        {
            var forecast = new TestForecast();

            var fixture = new Fixture();
            new AutoPropertiesCommand().Execute(forecast, new SpecimenContext(fixture));

            _serviceMock
                .Setup(z => z.GetForecast(It.IsAny<float>(), It.IsAny<float>(), default))
                .ReturnsAsync((forecast, nameof(_serviceMock)));

            var response = await _sut.Index(0, 0);

            Assert.True(response.Success);
            Assert.Equal(nameof(_serviceMock), response.Data.Provider);

            foreach (var property in typeof(IForecast).GetProperties())
                Assert.Equal(property.GetValue(forecast), typeof(ForecastVM).GetProperty(property.Name).GetValue(response.Data));
        }
    }
}
