using System;
using System.Threading;
using System.Threading.Tasks;

namespace Weather.Domain.Interfaces
{
    public interface IWeatherLogger
    {
        Task Log(IForecast forecast, string provider, TimeSpan elapsed, CancellationToken ct = default);
        Task LogError(Exception e, TimeSpan elapsed, CancellationToken ct = default);
    }
}
