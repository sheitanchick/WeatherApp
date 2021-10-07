using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Weather.Application.Interfaces;
using Weather.Domain.Interfaces;
using Weather.WebApi.ViewModels;

namespace Weather.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForecastController : ControllerBase
    {
        private readonly IForecastService _forecastService;
        private readonly IMapper _mapper;

        public ForecastController(IForecastService forecastService, IMapper mapper)
        {
            _forecastService = forecastService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<WeatherResponse<ForecastVM>> Index([Range(-90, 90)] float lat, [Range(-180, 180)] float lon)
        {
            (IForecast Forecast, string Provider) = await _forecastService.GetForecast(lat, lon, HttpContext.RequestAborted);
            
            var response = new WeatherResponse<ForecastVM>
            {
                Success = true,
                Data = _mapper.Map<IForecast, ForecastVM>(Forecast, opt => opt.AfterMap((s, d) => d.Provider = Provider))
            };

            return response;
        }
    }
}
