using System.Text.Json.Serialization;

namespace RestSharp.ApiTests.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [JsonPropertyName("is_admin")]
        public int IsAdmin { get; set; }

        [JsonPropertyName("sir_name")]
        public string SirName { get; set; }

        public string Title { get; set; }
    }
}
