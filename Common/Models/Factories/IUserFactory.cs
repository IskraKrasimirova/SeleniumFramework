namespace Common.Models.Factories
{
    public interface IUserFactory
    {
        T CreateDefault<T>() where T : UserModel, new();
        UserModel Create(string email, string password);
        T CreateCustom<T>(
            string? title = null, 
            string? firstName = null,
            string? surname = null, 
            string? email = null, 
            string? password = null, 
            string? country = null, 
            string? city = null) 
            where T : UserModel, new();
    }
}
