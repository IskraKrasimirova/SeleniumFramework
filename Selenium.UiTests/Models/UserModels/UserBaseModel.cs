namespace Selenium.UiTests.Models.UserModels
{
    public abstract class UserBaseModel
    {
        public string Title { get; protected set; }
        public string FirstName { get; protected set; }
        public string Surname { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Country { get; protected set; }
        public string City { get; protected set; }

        protected UserBaseModel(string title, string firstName, string surname, string email, string password, string country, string city)
        {
            Title = title;
            FirstName = firstName;
            Surname = surname;
            Email = email;
            Password = password;
            Country = country;
            City = city;
        }

        public void Set(
            string? title = null,
            string? firstName = null,
            string? surname = null,
            string? email = null,
            string? password = null,
            string? country = null,
            string? city = null)
        {
            if (title != null) Title = title;
            if (firstName != null) FirstName = firstName;
            if (surname != null) Surname = surname;
            if (email != null) Email = email;
            if (password != null) Password = password;
            if (country != null) Country = country;
            if (city != null) City = city;
        }
    }
}
