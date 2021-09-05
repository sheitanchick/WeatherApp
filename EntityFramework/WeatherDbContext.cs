using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.EntityFramework
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        { }

        public DbSet<WeatherLog> WeatherLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherLog>(typeBuilder => 
            {
                typeBuilder.ToTable(nameof(WeatherLogs));
                typeBuilder.HasKey(z => z.Id);
            });

            modelBuilder.Entity<WeatherLogData>(typeBuilder =>
            {
                typeBuilder.ToTable(nameof(WeatherLogData));
                typeBuilder.HasKey(z => z.WeatherLogId);
                typeBuilder.HasOne(z => z.WeatherLog)
                    .WithOne(z => z.Data)
                    .HasForeignKey<WeatherLogData>(z => z.WeatherLogId);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity;

                if (entry.State == EntityState.Added
                        && entity is IHaveCreatedTime canProvideCreated && canProvideCreated.Created.Equals(default))
                    canProvideCreated.Created = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
