using Bogus;
using Bogus.Extensions;

namespace Common.Models.Factories
{
    public class UsersFactory : IUserFactory
    {
        private static readonly Faker Faker = new();
        private static readonly string[] Titles = ["Mr.", "Mrs."];
        private static readonly List<string> ValidCities = ["Burgas", "Elin Pelin", "Kardjali", "Pleven", "Plovdiv", "Pravets", "Sofia", "Sopot", "Varna"];

        public T CreateDefault<T>() where T : UserModel, new()
        {
            var user = new T
            {
                Title = Faker.PickRandom(Titles),
                FirstName = Faker.Name.FirstName().Replace("'", "").ClampLength(2, 15),
                Surname = Faker.Name.LastName().Replace("'", "").ClampLength(2, 15),
                Email = Faker.Internet.Email(),
                Password = Faker.Internet.Password(),
                Country = "Bulgaria",
                City = Faker.PickRandom(ValidCities)
            };

            return user;
        }

        public UserModel Create(string email, string password)
        {
            UserModel user = CreateDefault<UserModel>();
            user.Email = email;
            user.Password = password;

            return user;
        }

        public T CreateCustom<T>(
            string? title = null,
            string? firstName = null,
            string? surname = null,
            string? email = null,
            string? password = null,
            string? country = null,
            string? city = null)
            where T : UserModel, new()
        {
            var user = CreateDefault<T>();

            if (title != null) user.Title = title;
            if (firstName != null) user.FirstName = firstName;
            if (surname != null) user.Surname = surname;
            if (email != null) user.Email = email;
            if (password != null) user.Password = password;
            if (country != null) user.Country = country;
            if (city != null) user.City = city;

            return user;
        }
    }
}
