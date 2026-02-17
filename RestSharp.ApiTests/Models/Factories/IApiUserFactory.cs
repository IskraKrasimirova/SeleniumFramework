using RestSharp.ApiTests.Models.Dtos;

namespace RestSharp.ApiTests.Models.Factories
{
    public interface IApiUserFactory
    {
        UserDto CreateDefault();
        UserDto CreateCustom(
            string? title = null,
            string? firstName = null,
            string? surname = null,
            string? email = null,
            string? password = null,
            string? country = null,
            string? city = null,
            int? isAdmin = null);
    }
}
