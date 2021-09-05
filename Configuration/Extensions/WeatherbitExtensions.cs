using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Weather.Providers;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;

namespace Weather.Configuration.Extensions
{
    public static class WeatherbitExtensions
    {
        public static IServiceCollection AddWeatherbit(this IServiceCollection services)
        {
            services
                .AddTransient<WeatherbitDelegatingHandler>()
                .AddHttpClient<IForecastProvider, WeatherbitForecastProvider>(nameof(WeatherbitForecastProvider), (services, client) =>
                {
                    var config = services.GetRequiredService<IOptions<WeatherbitOptions>>().Value;

                    client.BaseAddress = config.BaseUrl;

                })
                .AddHttpMessageHandler<WeatherbitDelegatingHandler>();

            return services;
        }
    }

    public class WeatherbitDelegatingHandler : DelegatingHandler
    {
        private readonly WeatherbitOptions _config;

        public WeatherbitDelegatingHandler(IOptions<WeatherbitOptions> weatherbitOptions)
        {
            _config = weatherbitOptions.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);

            var query = QueryHelpers.ParseQuery(uriBuilder.Query);

            query.TryAdd("key", _config.ApiKey);

            uriBuilder.Query = QueryHelpers.AddQueryString("", query.ToDictionary(z => z.Key, z => z.Value.ToString()));

            request.RequestUri = uriBuilder.Uri;

            var response = await base.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
