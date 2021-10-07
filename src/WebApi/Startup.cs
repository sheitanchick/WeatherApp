using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using Weather.Application;
using Weather.Application.Interfaces;
using Weather.WebApi.Extensions;
using Weather.Domain.Interfaces;
using Weather.Infrastructure.OpenWeather;
using Weather.Infrastructure.Persistence;
using Weather.Infrastructure.Repositories;
using Weather.Infrastructure.Weatherbit;
using Weather.WebApi.Utility;

namespace Weather.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services
                .AddDbContext<WeatherDbContext>(Configuration)
                .AddAutoMapper(typeof(Startup).Assembly)
                .AddScoped<IForecastService, ForecastService>()
                .Decorate<IForecastService, ForecastLoggingService>()
                .AddWeatherbit()
                .AddOpenWeather()
                .Configure<WeatherbitOptions>(options => Configuration.GetSection("Weatherbit").Bind(options))
                .Configure<OpenWeatherOptions>(options => Configuration.GetSection("OpenWeather").Bind(options))
                .AddScoped<IWeatherLogger, WeatherLogger>()
                .AddSwaggerGen(c => 
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather service API", Version = "v1" });
                })
                .AddControllers(c => c.Filters.Add<ExceptionHandlingFilter>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API");
                })
                .Use((ctx, next) =>
                {
                    if (ctx.Request.Path != "/") return next.Invoke();
                    ctx.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
