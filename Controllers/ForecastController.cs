using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.DTO;
using Weather.Providers;

namespace Weather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForecastController : ControllerBase
    {
        [HttpGet]
        public async Task<WeatherDto> Index(float lat, float lon, [FromServices] IEnumerable<IForecastProvider> forecastProviders)
        {
            var parameters = new Dictionary<string, string>
            {
                ["lat"] = lat.ToString(),
                ["lon"] = lon.ToString(),
            };

            var requests = forecastProviders.Select(p => p.GetWeatherForecast(parameters, HttpContext.RequestAborted));

            var first = await Task.WhenAny(requests);

            return await first;
        }
    }
}
