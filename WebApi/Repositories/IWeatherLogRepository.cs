using System.Threading;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Repositories
{
    public interface IWeatherLogRepository
    {
        void Add(WeatherLog log);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
