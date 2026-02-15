namespace SeleniumTestFramework.UiTests.DatabaseOperations.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; } = "uploads/profile_pictures/default_profile.png";
    }
}
