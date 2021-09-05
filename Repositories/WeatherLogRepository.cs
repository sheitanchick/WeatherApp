using System.Threading;
using System.Threading.Tasks;
using Weather.EntityFramework;
using Weather.Models;

namespace Weather.Repositories
{
    public class WeatherLogRepository : IWeatherLogRepository
    {
        private readonly WeatherDbContext _dbContext;

        public WeatherLogRepository(WeatherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(WeatherLog log)
        {
            _dbContext.Add(log);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return _dbContext.SaveChangesAsync(ct);
        }
    }
}
