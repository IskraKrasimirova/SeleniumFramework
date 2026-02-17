using Common.Models.SettingsModels;
using Microsoft.Extensions.Configuration;

namespace Common.Utilities
{
    public class ConfigurationManager<TSettingsModel> where TSettingsModel : BaseSettingsModel
    {
        private static readonly Lazy<ConfigurationManager<TSettingsModel>> lazy =
            new(() => new ConfigurationManager<TSettingsModel>());

        public static ConfigurationManager<TSettingsModel> Instance { get; } = lazy.Value;

        public TSettingsModel SettingsModel { get; }

        private ConfigurationManager()
        {
            var environment = Environment.GetEnvironmentVariable("environment", EnvironmentVariableTarget.User);
            var configurationFileName = string.IsNullOrEmpty(environment)
                ? "appsettings.json"
                : $"appsettings.{environment}.json";

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(configurationFileName)
                .Build();

            SettingsModel = config.GetSection("Settings").Get<TSettingsModel>()!;
        }

        public TSettingsModel LoadSection(string sectionName) 
        { 
            var config = BuildConfiguration();
            return config.GetSection(sectionName).Get<TSettingsModel>()!; 
        }

        private IConfiguration BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("environment", EnvironmentVariableTarget.User);
            var configurationFileName = string.IsNullOrEmpty(environment)
                ? "appsettings.json"
                : $"appsettings.{environment}.json";

            return new ConfigurationBuilder()
                .AddJsonFile(configurationFileName)
                .Build();
        }
    }
}
