using Newtonsoft.Json;

namespace DatabaseApi.Models;

public class MessageLogger
{
    [JsonProperty("index")]
    public string Index { get; set; } = string.Empty;
}
