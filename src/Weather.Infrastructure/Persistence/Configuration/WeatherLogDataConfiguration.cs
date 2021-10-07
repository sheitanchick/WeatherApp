using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Persistence.Configuration
{
    internal class WeatherLogDataConfiguration : IEntityTypeConfiguration<WeatherLogData>
    {
        public void Configure(EntityTypeBuilder<WeatherLogData> builder)
        {
            builder.ToTable(nameof(WeatherLogData));
            builder.HasKey(z => z.WeatherLogId);
            builder.HasOne(z => z.WeatherLog)
                .WithOne(z => z.Data)
                .HasForeignKey<WeatherLogData>(z => z.WeatherLogId);
        }
    }
}
