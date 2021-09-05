using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Weather.DTO;

namespace Weather.Utility
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new WeatherResponse<object>
            {
                Success = false
            };

            context.Result = new ObjectResult(response);
        }
    }
}
