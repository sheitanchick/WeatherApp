using AutoFixture;
using AutoFixture.Kernel;
using AutoMapper;
using Weather.Domain.Interfaces;
using Weather.Tests.Implementations;
using Weather.WebApi;
using Weather.WebApi.ViewModels;
using Xunit;

namespace Weather.Tests.UnitTests
{
    public class Mapper_Tests
    {
        private readonly IMapper _sut;

        public Mapper_Tests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _sut = config.CreateMapper();
        }

        [Fact]
        public void MapperProfileIsValid()
        {
            _sut.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Mapper_CorrectlyMapsIforecastToForecastVM()
        {
            IForecast forecast = new TestForecast();
            
            var fixture = new Fixture();

            new AutoPropertiesCommand().Execute(forecast, new SpecimenContext(fixture));

            var vm = _sut.Map<ForecastVM>(forecast);

            Assert.NotNull(vm);
            Assert.Null(vm.Provider);

            foreach (var property in typeof(IForecast).GetProperties())
                Assert.Equal(property.GetValue(forecast), typeof(ForecastVM).GetProperty(property.Name).GetValue(vm));
        }
    }
}
