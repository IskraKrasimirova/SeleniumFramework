using Common.Models.SettingsModels;
using Common.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using RestSharp.ApiTests.Apis;
using RestSharp.ApiTests.Models.Factories;

namespace RestSharp.ApiTests.Hooks
{
    public class DependencyContainer
    {
        [ScenarioDependencies]
        public static IServiceCollection RegisterDependencies()
        {
            var services = new ServiceCollection();

            services.AddSingleton(sp =>
            {
                return ConfigurationManager<ApiSettingsModel>.Instance.SettingsModel;
            });

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<ApiSettingsModel>();

                var options = new RestClientOptions(settings.BaseUrl);
                var client = new RestClient(options);
                client.AddDefaultHeader("Accept", "application/json");
                return client;
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<RestClient>();
                return new UsersApi(client);
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<RestClient>();
                return new LoginApi(client);
            });

            services.AddSingleton<IApiUserFactory, ApiUserFactory>();

            return services;
        }
    }
}
