using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Weather.Domain.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Weather.Infrastructure.OpenWeather
{
    public static class OpenWeatherExtensions
    {
        public static IServiceCollection AddOpenWeather(this IServiceCollection services)
        {
            services
                .AddTransient<OpenWeatherDelegatingHandler>()
                .AddHttpClient<IForecastProvider, OpenWeatherForecastProvider>(nameof(OpenWeatherForecastProvider), (services, client) =>
                {
                    var config = services.GetRequiredService<IOptions<OpenWeatherOptions>>().Value;

                    client.BaseAddress = config.BaseUrl;
                })
                .AddHttpMessageHandler<OpenWeatherDelegatingHandler>();

            return services;
        }
    }

    public class OpenWeatherDelegatingHandler : DelegatingHandler
    {
        private readonly OpenWeatherOptions _config;

        public OpenWeatherDelegatingHandler(IOptions<OpenWeatherOptions> openWeatherOptions)
        {
            _config = openWeatherOptions.Value;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);

            var query = QueryHelpers.ParseQuery(uriBuilder.Query);

            query.TryAdd("appid", _config.ApiKey);

            uriBuilder.Query = QueryHelpers.AddQueryString("", query.ToDictionary(z => z.Key, z => z.Value.ToString()));

            request.RequestUri = uriBuilder.Uri;

            return base.SendAsync(request, cancellationToken);
        }
    }
}
