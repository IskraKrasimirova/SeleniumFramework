namespace SeleniumTestFramework.UiTests.Models.UserModels
{
    public class AddUserModel: UserBaseModel
    {
        public bool IsAdmin { get; private set; }

        public AddUserModel(string title, string firstName, string surname, string email, string password, string country, string city, bool isAdmin)
            : base(title, firstName, surname, email, password, country, city)
        {
            IsAdmin = isAdmin;
        }

        public void SetIsAdmin(bool? isAdmin = null)
        {
            if (isAdmin.HasValue)
                IsAdmin = isAdmin.Value;
        }
    }
}
