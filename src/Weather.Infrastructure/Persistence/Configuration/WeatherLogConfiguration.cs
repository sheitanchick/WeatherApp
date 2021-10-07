using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Persistence.Configuration
{
    internal class WeatherLogConfiguration : IEntityTypeConfiguration<WeatherLog>
    {
        public void Configure(EntityTypeBuilder<WeatherLog> builder)
        {
            builder.ToTable(nameof(WeatherDbContext.WeatherLogs));
            builder.HasKey(z => z.Id);
        }
    }
}
