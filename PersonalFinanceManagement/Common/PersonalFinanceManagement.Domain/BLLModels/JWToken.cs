using System.Text.Json.Serialization;

namespace PersonalFinanceManagement.Domain.BLLModels
{
    public class JWToken
    {
        [JsonPropertyName("id_token")]
        public string Token { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("expires_in")]
        public int TokenExpiresInSeconds { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}
