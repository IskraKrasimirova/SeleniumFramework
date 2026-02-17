using Bogus;
using Bogus.Extensions;
using Selenium.UiTests.Models.UserModels;

namespace Selenium.UiTests.Utilities
{
    public static class UserFactory
    {
        private static readonly Faker Faker = new();
        private static readonly string[] Titles = ["Mr.", "Mrs."];
        private static readonly List<string> ValidCities = ["Burgas", "Elin Pelin", "Kardjali", "Pleven", "Plovdiv", "Pravets", "Sofia", "Sopot", "Varna"];


        public static RegisterModel CreateValidUser()
        {
            return new RegisterModel
            (
                Faker.PickRandom(Titles),
                Faker.Name.FirstName().Replace("'", "").ClampLength(2, 15),
                Faker.Name.LastName().Replace("'", "").ClampLength(2, 15),
                Faker.Internet.Email(),
                Faker.Internet.Password(),
                "Bulgaria",
                Faker.PickRandom(ValidCities),
                true
            );
        }

        public static RegisterModel CreateUserWith(Action<RegisterModel> overrides)
        {
            var user = CreateValidUser();

            overrides(user);
            return user;
        }

        public static AddUserModel CreateValidCommonUser()
        {
            return new AddUserModel
            (
                Faker.PickRandom(Titles),
                Faker.Name.FirstName().Replace("'", "").ClampLength(2, 15),
                Faker.Name.LastName().Replace("'", "").ClampLength(2, 15),
                Faker.Internet.Email(),
                Faker.Internet.Password(),
                "Bulgaria",
                Faker.PickRandom(ValidCities),
                false
            );
        }

        public static AddUserModel CreateUserWith(Action<AddUserModel> overrides)
        {
            var user = CreateValidCommonUser();

            overrides(user);
            return user;
        }

        //// Generic method to create a user of type T with overrides
        //public static T CreateUserWith<T>(Action<T> overrides) where T : UserBaseModel 
        //{ 
        //    T user = CreateValid<T>(); 
        //    overrides(user);
        //    return user; 
        //}

        //private static T CreateValid<T>() where T : UserBaseModel 
        //{ 
        //    if (typeof(T) == typeof(RegisterModel)) 
        //        return (T)(UserBaseModel)CreateValidUser(); 
            
        //    if (typeof(T) == typeof(AddUserModel)) 
        //        return (T)(UserBaseModel)CreateValidCommonUser(); 
            
        //    throw new NotSupportedException($"Unsupported model type: {typeof(T).Name}"); 
        //}
    }
}
