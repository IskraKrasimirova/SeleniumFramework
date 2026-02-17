namespace Common.Models.Builders
{
    public class UserBuilder
    {
        private readonly UserModel _userModel;

        public UserBuilder()
        {
            _userModel = new UserModel();
        }

        public UserBuilder WithDefaultValues()
        {
            _userModel.Title = "Mr.";
            _userModel.FirstName = "John";
            _userModel.Surname = "Doe";
            _userModel.Email = $"john.doe{Guid.NewGuid()}@example.com";
            _userModel.Password = "SecureP@ssw0rd";
            _userModel.Country = "USA";
            _userModel.City = "New York";

            return this;
        }

        public UserBuilder WithTitle(string title)
        {
            _userModel.Title = title;
            return this;
        }
        public UserBuilder WithFirstName(string firstName)
        {
            _userModel.FirstName = firstName;
            return this;
        }
        public UserBuilder WithSurname(string surname)
        {
            _userModel.Surname = surname;
            return this;
        }
        public UserBuilder WithEmail(string email)
        {
            _userModel.Email = email;
            return this;
        }
        public UserBuilder WithPassword(string password)
        {
            _userModel.Password = password;
            return this;
        }
        public UserBuilder WithCountry(string country)
        {
            _userModel.Country = country;
            return this;
        }
        public UserBuilder WithCity(string city)
        {
            _userModel.City = city;
            return this;
        }

        public UserModel Build()
        {
            return _userModel;
        }
    }
}
