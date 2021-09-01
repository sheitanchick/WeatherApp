using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Weather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Index()
        {
            return await Task.FromResult("Hello");
        }
    }
}
