using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Weather.DTO;
using Weather.Providers;
using Weather.Utility;

namespace Weather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForecastController : ControllerBase
    {
        private readonly IEnumerable<IForecastProvider> _forecastProviders;

        public ForecastController(IEnumerable<IForecastProvider> forecastProviders)
        {
            _forecastProviders = forecastProviders;
        }

        [HttpGet]
        [TypeFilter(typeof(WeatherLoggingActionFilter))]
        public async Task<WeatherResponse<WeatherDto>> Index([Range(-90, 90)] float lat, [Range(-180, 180)] float lon)
        {
            var parameters = new Dictionary<string, string>
            {
                ["lat"] = lat.ToString(),
                ["lon"] = lon.ToString()
            };

            var requests = _forecastProviders
                .Select(p => p.GetWeatherForecast(parameters, HttpContext.RequestAborted))
                .ToArray();

            var weather = await requests.GetFirstSuccessfullyExecutedTask();

            var response = new WeatherResponse<WeatherDto>
            {
                Success = true,
                Data = await weather
            };

            return response;
        }
    }
}
