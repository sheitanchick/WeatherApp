using System;

namespace Weather.Models
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
