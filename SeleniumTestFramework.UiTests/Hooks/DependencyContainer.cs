using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using RestSharp;
using SeleniumTestFramework.ApiTests.Apis;
using SeleniumTestFramework.UiTests.Pages;
using SeleniumTestFramework.UiTests.DatabaseOperations.Operations;
using SeleniumTestFramework.UiTests.Models;
using SeleniumTestFramework.UiTests.Models.Builders;
using SeleniumTestFramework.UiTests.Models.Factories;
using System.Data;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using ConfigurationManager = SeleniumTestFramework.UiTests.Utilities.ConfigurationManager;

namespace SeleniumTestFramework.UiTests.Hooks
{
    public class DependencyContainer
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton(sp =>
            {
                return ConfigurationManager.Instance.SettingsModel;
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
            RegisterApiIntegration(services);

            return services;
        }

        private static void RegisterDatabaseOperations(ServiceCollection services)
        {
            services.AddScoped<IDbConnection>(sp =>
            {
                var settings = sp.GetRequiredService<SettingsModel>();
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
                var options = new RestClientOptions(ConfigurationManager.Instance.SettingsModel.ApiBaseUrl);
                var client = new RestClient(options);
                client.AddDefaultHeader("Accept", "application/json");
                return client;
            });

            services.AddScoped<UsersApi>();

            // API UserFactory
            services.AddSingleton<ApiTests.Models.Factories.IUserFactory, ApiTests.Models.Factories.UserFactory>();
        }
    }
}
