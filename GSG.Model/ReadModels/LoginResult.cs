using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GSG.Model.ReadModels
{
    public class LoginResult
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
