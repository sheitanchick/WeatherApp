using System;
using Weather.Domain.Interfaces;

namespace Weather.Domain.Entities
{
    public class WeatherLog : IHaveCreatedTime
    {
        public long Id { get; set; }
        public TimeSpan Elapsed { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Created { get; set; }

        public WeatherLogData Data { get; set; }
    }
}
