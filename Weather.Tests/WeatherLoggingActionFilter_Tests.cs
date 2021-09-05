using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weather.DTO;
using Weather.Models;
using Weather.Repositories;
using Weather.Utility;
using Xunit;

namespace Weather.Tests
{
    public class WeatherLoggingActionFilter_Tests
    {
        private readonly Mock<IWeatherLogRepository> _logRepositoryMock;
        private readonly ActionExecutedContext _fakeActionExecutedContext;
        private readonly WeatherLoggingActionFilter _sut;

        public WeatherLoggingActionFilter_Tests()
        {
            _logRepositoryMock = new Mock<IWeatherLogRepository>();
            
            _fakeActionExecutedContext = new ActionExecutedContext(
                new ActionContext(
                    httpContext: new DefaultHttpContext(),
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor()
                ),
                new List<IFilterMetadata>(),
                new Mock<Controller>().Object);

            _sut = new WeatherLoggingActionFilter(_logRepositoryMock.Object);
        }

        [Fact]
        public async Task OnActionExecutionAsync_CreateLogWithDataWhenResultIsOk() 
        {
            var data = new WeatherResponse<WeatherDto>
            {
                Success = true,
                Data = new WeatherDto 
                {
                    Provider = "test provider"
                }
            };

            var result = new ObjectResult(data);

            _fakeActionExecutedContext.Result = result;

            Task<ActionExecutedContext> next() => Task.FromResult(_fakeActionExecutedContext);

            await _sut.OnActionExecutionAsync(null, next);

            _logRepositoryMock.Verify(z => z.Add(It.Is<WeatherLog>(log => log.Data.Provider == data.Data.Provider)), Times.Once);
            _logRepositoryMock.Verify(z => z.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);  
        }

        [Fact]
        public async Task OnActionExecutionAsync_CreateLogWithoutDataWhenResultIsNotSuccessful()
        {
            var data = new WeatherResponse<WeatherDto>
            {
                Success = false
            };

            var result = new ObjectResult(data);
            
            _fakeActionExecutedContext.Result = result;

            Task<ActionExecutedContext> next() => Task.FromResult(_fakeActionExecutedContext);

            await _sut.OnActionExecutionAsync(null, next);

            _logRepositoryMock.Verify(z => z.Add(It.Is<WeatherLog>(log => log != null)), Times.Once);
            _logRepositoryMock.Verify(z => z.Add(It.Is<WeatherLog>(log => log.Data == null)), Times.Once);
            _logRepositoryMock.Verify(z => z.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
