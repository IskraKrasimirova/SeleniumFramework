using RestSharp.ApiTests.Models.Dtos;

namespace RestSharp.ApiTests.Apis
{
    public class UsersApi
    {
        private readonly RestClient _restClient;
        private readonly string _uri;

        public UsersApi(RestClient restClient)
        {
            _restClient = restClient;
            _uri = "/users";
        }

        public RestResponse<UserDto> GetUserById(int id)
        {
            var request = new RestRequest($"{_uri}/{id}", Method.Get);
            var response = _restClient.Execute<UserDto>(request);
            return response;
        }

        public RestResponse<T> CreateUser<T>(object expectedUser) where T : notnull
        {
            var request = new RestRequest(_uri, Method.Post);
            request.AddJsonBody(expectedUser);
            return _restClient.Execute<T>(request);
        }

        public RestResponse DeleteUserById(int id)
        {
            var request = new RestRequest($"{_uri}/{id}", Method.Delete);
            return _restClient.Execute(request);
        }

        public RestResponse<UserDto> UpdateUser(int id, UserDto updatedData)
        {
            var request = new RestRequest($"{_uri}/{id}", Method.Put);
            request.AddJsonBody(updatedData);
            return _restClient.Execute<UserDto>(request);
        }

        public RestResponse<IReadOnlyCollection<UserDto>> GetAllUsers()
        {
            var request = new RestRequest(_uri, Method.Get);
            return _restClient.Execute<IReadOnlyCollection<UserDto>>(request);
        }
    }
}
