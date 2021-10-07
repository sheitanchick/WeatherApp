using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Weather.Domain.Entities;
using Weather.Domain.Interfaces;
using Weather.Infrastructure.Persistence;
using Weather.Infrastructure.Repositories;
using Weather.Infrastructure.Weatherbit;
using Xunit;

namespace Weather.Tests.UnitTests
{
    public class WeatherLogger_Tests
    {
        private readonly WeatherDbContext _context;
        private readonly WeatherLogger _sut;
        
        public WeatherLogger_Tests()
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new WeatherDbContext(options);
            
            _sut = new WeatherLogger(_context);
        }

        [Fact]
        public async Task Log_SavesDataToDB()
        {
            var fixture = new Fixture();

            IForecast forecast = new WeatherbitResponse();

            new AutoPropertiesCommand().Execute(forecast, new SpecimenContext(fixture));

            var provider = "test_provider";
            var elapsed = TimeSpan.FromSeconds(1.4);
            
            await _sut.Log(forecast, provider, elapsed, default);

            var log = await _context.WeatherLogs.SingleAsync();

            Assert.Null(log.Message);
            Assert.Null(log.StackTrace);
            Assert.Equal(log.Data.Provider, provider);
            Assert.Equal(log.Elapsed, elapsed);

            foreach(var prop in typeof(IForecast).GetProperties())
                Assert.Equal(prop.GetValue(forecast), typeof(WeatherLogData).GetProperty(prop.Name).GetValue(log.Data));
        }

        [Fact]
        public async Task LogError_SavesExceptionInfo()
        {
            var elapsed = TimeSpan.FromSeconds(1.4);
            var exception = new Exception("exception_message");
            
            typeof(Exception)
                .GetField("_stackTraceString", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(exception, Environment.StackTrace);

            await _sut.LogError(exception, elapsed, default);

            var log = await _context.WeatherLogs.SingleAsync();

            Assert.Equal(log.Message, exception.Message);
            Assert.Equal(log.StackTrace, exception.StackTrace);
            Assert.Equal(log.Elapsed, elapsed);
            Assert.Null(log.Data);
        }
    }
}
