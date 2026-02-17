namespace Common.Models.SettingsModels
{
    public class UiSettingsModel : BaseSettingsModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string ConnectionString { get; set; }
    }
}
