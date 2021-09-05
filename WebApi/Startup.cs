using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using Weather.Configuration;
using Weather.Configuration.Extensions;
using Weather.EntityFramework;
using Weather.Repositories;
using Weather.Utility;

namespace Weather
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
            services
                .AddDbContext<WeatherDbContext>(Configuration)
                .AddWeatherbit()
                .AddOpenWeather()
                .Configure<WeatherbitOptions>(options => Configuration.GetSection("Weatherbit").Bind(options))
                .Configure<OpenWeatherOptions>(options => Configuration.GetSection("OpenWeather").Bind(options))
                .AddScoped<IWeatherLogRepository, WeatherLogRepository>()
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
            });
        }
    }
}
