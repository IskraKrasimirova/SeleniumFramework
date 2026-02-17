using Common.Models.Builders;
using Common.Models.Factories;
using Common.Models.SettingsModels;
using Common.Utilities;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using RestSharp;
using RestSharp.ApiTests.Apis;
using RestSharp.ApiTests.Models.Factories;
using Selenium.UiTests.DatabaseOperations.Operations;
using Selenium.UiTests.Pages;
using System.Data;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Selenium.UiTests.Hooks
{
    public class DependencyContainer
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton(sp =>
            {
                return ConfigurationManager<UiSettingsModel>.Instance.SettingsModel;
            });

            services.AddScoped<IWebDriver>(sp =>
            {
                new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

                var driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

                return driver;
            });

            services.AddSingleton<IUserFactory, UsersFactory>();
            services.AddScoped<UserBuilder>();

            RegisterPages(services);
            RegisterDatabaseOperations(services);

            services.AddSingleton(sp => ConfigurationManager<ApiSettingsModel>.Instance.LoadSection("ApiSettings"));

            RegisterApiIntegration(services);

            return services;
        }

        private static void RegisterDatabaseOperations(ServiceCollection services)
        {
            services.AddScoped<IDbConnection>(sp =>
            {
                var settings = sp.GetRequiredService<UiSettingsModel>();
                var connectionString = settings.ConnectionString;

                var dbConnection = new MySqlConnection(connectionString);
                dbConnection.Open();

                return dbConnection;
            });

            services.AddScoped(sp =>
            {
                var dbConnection = sp.GetRequiredService<IDbConnection>();
                return new UserOperations(dbConnection);
            });

            services.AddScoped(sp =>
            {
                var dbConnection = sp.GetRequiredService<IDbConnection>();
                return new SkillOperations(dbConnection);
            });

            services.AddScoped(sp =>
            {
                var dbConnection = sp.GetRequiredService<IDbConnection>();
                return new LocationOperations(dbConnection);
            });
        }

        private static void RegisterPages(ServiceCollection services)
        {
            services.AddScoped(sp =>
            {
                var driver = sp.GetRequiredService<IWebDriver>();
                return new LoginPage(driver);
            });

            services.AddScoped(sp =>
            {
                var driver = sp.GetRequiredService<IWebDriver>();
                return new RegisterPage(driver);
            });

            services.AddScoped(sp =>
            {
                var driver = sp.GetRequiredService<IWebDriver>();
                return new DashboardPage(driver);
            });

            services.AddScoped(sp =>
            {
                var driver = sp.GetRequiredService<IWebDriver>();
                return new UsersPage(driver);
            });

            services.AddScoped(sp =>
            {
                var driver = sp.GetRequiredService<IWebDriver>();
                return new AddUserModalPage(driver);
            });

            services.AddScoped(sp =>
            {
                var driver = sp.GetRequiredService<IWebDriver>();
                return new SearchPage(driver);
            });

            services.AddScoped(sp =>
            {
                var driver = sp.GetRequiredService<IWebDriver>();
                return new SearchResultPage(driver);
            });

            // Short syntax for registering page classes
            //services.AddScoped<LoginPage>();
            //services.AddScoped<DashboardPage>();
            //services.AddScoped<RegisterPage>();
            //services.AddScoped<UsersPage>();
        }

        private static void RegisterApiIntegration(IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<ApiSettingsModel>();

                var options = new RestClientOptions(settings.BaseUrl);
                var client = new RestClient(options);
                client.AddDefaultHeader("Accept", "application/json");
                return client;
            });

            services.AddScoped<UsersApi>();

            services.AddSingleton<IApiUserFactory, ApiUserFactory>();
        }
    }
}
