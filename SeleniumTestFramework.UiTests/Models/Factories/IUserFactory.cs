namespace SeleniumTestFramework.UiTests.Models.Factories
{
    public interface IUserFactory
    {
        UserModel CreateDefault();
        UserModel Create(string email, string password);
        UserModel CreateCustom(
            string? title = null,
            string? firstName = null,
            string? surname = null,
            string? email = null,
            string? password = null,
            string? country = null,
            string? city = null);
    }
}
