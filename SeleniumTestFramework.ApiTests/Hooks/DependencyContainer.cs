using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using RestSharp;
using SeleniumTestFramework.ApiTests.Apis;
using SeleniumTestFramework.ApiTests.Models.Factories;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SeleniumTestFramework.ApiTests.Hooks
{
    public class DependencyContainer
    {
        [ScenarioDependencies]
        public static IServiceCollection RegisterDependencies()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build(); 

            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<RestClient>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var baseUrl = config["ApiBaseUrl"]!;
                
                var options = new RestClientOptions(baseUrl);
                var client = new RestClient(options);
                client.AddDefaultHeader("Accept", "application/json");
                return client;
            });

            services.AddSingleton<IUserFactory, UserFactory>();

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

            return services;
        }
    }
}
