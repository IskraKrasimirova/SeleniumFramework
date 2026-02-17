using Common.Models.SettingsModels;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using Selenium.UiTests.Hooks;

namespace Selenium.UiTests.Tests
{
    public abstract class UiTestBase
    {
        protected ServiceProvider Provider { get; private set; }
        protected IWebDriver Driver { get; private set; }
        protected UiSettingsModel Settings { get; private set; }
        protected IServiceScope TestScope { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var services = DependencyContainer.CreateServices(); 
            Provider = services.BuildServiceProvider(); 
            Settings = Provider.GetRequiredService<UiSettingsModel>();
        }

        [SetUp]
        public void Setup()
        {
            TestScope = Provider.CreateScope();
            Driver = TestScope.ServiceProvider.GetRequiredService<IWebDriver>();
        }

        [TearDown]
        public void Teardown()
        {
            Driver.Quit();
            Driver.Dispose();
            TestScope.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            Provider.Dispose();
        }
    }
}
