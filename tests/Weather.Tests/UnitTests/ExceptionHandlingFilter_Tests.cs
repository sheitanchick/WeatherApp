using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Weather.WebApi.Utility;
using Weather.WebApi.ViewModels;
using Xunit;

namespace Weather.Tests.UnitTests
{
    public class ExceptionHandlingFilter_Tests
    {
        private readonly ExceptionHandlingFilter _sut;
        private readonly ExceptionContext _exceptionContext;

        public ExceptionHandlingFilter_Tests()
        {
            var _fakeActionContext = new ActionContext(
                    httpContext: new DefaultHttpContext(),
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor()
                );

            _exceptionContext = new ExceptionContext(_fakeActionContext, new List<IFilterMetadata>());

            _sut = new ExceptionHandlingFilter();
        }

        [Fact]
        public void OnException_ReturnsResponseWithFalsyStatus()
        {
            var data = new WeatherResponse<object>();
            var result = new ObjectResult(data);

            _sut.OnException(_exceptionContext);

            _exceptionContext.Result.Should().NotBeNull();
            _exceptionContext.Result.Should().BeEquivalentTo(result);
        }
    }
}
