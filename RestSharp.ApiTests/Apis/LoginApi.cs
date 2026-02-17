using RestSharp.ApiTests.Models.Dtos;

namespace RestSharp.ApiTests.Apis
{
    public class LoginApi
    {
        private readonly RestClient _restClient;
        private readonly string _uri;

        public LoginApi(RestClient restClient)
        {
            _restClient = restClient;
            _uri = "/login";
        }

        public RestResponse<UserDto> LoginWith(LoginDto existingUserCredentials)
        {
            var request = new RestRequest(_uri, Method.Post);
            request.AddJsonBody(existingUserCredentials);
            var response = _restClient.Execute<UserDto>(request);
            return response;
        }

        public RestResponse LoginWithGet(LoginDto loginDto)
        {
            var request = new RestRequest(_uri, Method.Get);

            request.AddQueryParameter("email", loginDto.Email);
            request.AddQueryParameter("password", loginDto.Password);

            return _restClient.Execute(request);
        }

        public RestResponse LoginWithEmptyBody()
        {
            var request = new RestRequest(_uri, Method.Post);

            return _restClient.Execute(request);
        }

        public RestResponse LoginWithRawJson(string rawJson)
        {
            var request = new RestRequest(_uri, Method.Post);
            request.AddStringBody(rawJson, ContentType.Json);

            return _restClient.Execute(request);
        }
    }
}
