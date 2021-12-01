using Newtonsoft.Json;

namespace ASM.Core.Models
{
    public class AccessToken : RequestResponseBase
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }
        [JsonProperty("token_type")]
        public string token_type { get; set; }
        [JsonProperty("expires_in")]
        public int expires_in { get; set; }
        [JsonProperty("scope")]
        public string scope { get; set; }
        [JsonProperty("user_id")]
        public int user_id { get; set; }
        [JsonProperty("refresh_token")]
        public string refresh_token { get; set; }
    }
}
