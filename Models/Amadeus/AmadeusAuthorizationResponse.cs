using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Amadeus;

public class AmadeusAuthorizationResponse
{
    [JsonProperty("type")]
    public string Type { get; set; } = null!;
    [JsonProperty("username")]
    public string UserName { get; set; } = null!;
    [JsonProperty("application_name")]
    public string ApplicationName { get; set; } = null!;
    [JsonProperty("client_id")]
    public string ClientId { get; set; } = null!;
    [JsonProperty("token_type")]
    public string TokenType { get; set; } = null!;
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = null!;
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonProperty("state")]
    public string State { get; set; } = null!;
    [JsonProperty("scope")]
    public string Scope { get; set; } = null!;
}

