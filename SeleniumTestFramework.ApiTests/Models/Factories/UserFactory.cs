using Bogus;
using SeleniumTestFramework.ApiTests.Models.Dtos;
using SeleniumTestFramework.ApiTests.Utils.Types;

namespace SeleniumTestFramework.ApiTests.Models.Factories
{
    public class UserFactory: IUserFactory
    {
        private static readonly Faker Faker = new();
        private static readonly string[] Titles = ["Mr.", "Mrs."];
        private static readonly List<string> ValidCities = ["Burgas", "Elin Pelin", "Kardjali", "Pleven", "Plovdiv", "Pravets", "Sofia", "Sopot", "Varna"];

        public UserDto CreateDefault()
        {
            var plainPassword = Faker.Internet.Password();

            var user = new UserDto
            {
                Title = Faker.PickRandom(Titles),
                FirstName = Faker.Name.FirstName().Replace("'", ""),
                SirName = Faker.Name.LastName().Replace("'", ""),
                Email = Faker.Internet.Email(),
                Password = StringUtils.Sha256(plainPassword),
                Country = "Bulgaria",
                City = Faker.PickRandom(ValidCities),
                IsAdmin = 0
            };

            return user;
        }

        public UserDto CreateCustom(
            string? title = null,
            string? firstName = null,
            string? surname = null,
            string? email = null,
            string? password = null,
            string? country = null,
            string? city = null,
            int? isAdmin=null)
        {
            var user = CreateDefault();

            if (title != null) user.Title = title;
            if (firstName != null) user.FirstName = firstName;
            if (surname != null) user.SirName = surname;
            if (email != null) user.Email = email;
            if (password != null) user.Password = StringUtils.Sha256(password);
            if (country != null) user.Country = country;
            if (city != null) user.City = city;
            if (isAdmin.HasValue) user.IsAdmin = isAdmin.Value;

            return user;
        }
    }
}
