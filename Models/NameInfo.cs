
using System.Text.Json.Serialization;

namespace LazyAPI.Models;

public class NameInfo
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("birthdate")]
    public string Birthdate { get; set; }
}
