namespace SeleniumTestFramework.UiTests.Models.UserModels
{
    public class RegisterModel: UserBaseModel
    {
        public bool AgreeToTerms { get; private set; }

        public RegisterModel(string title, string firstName, string surname, string email, string password, string country, string city, bool agreeToTerms) : base(title, firstName, surname, email, password, country, city)
        {
            AgreeToTerms = agreeToTerms;
        }

        public void SetAgreeToTerms(bool? agreeToTerms = null) 
        { 
            if (agreeToTerms.HasValue) 
                AgreeToTerms = agreeToTerms.Value; 
        }
    }
}
