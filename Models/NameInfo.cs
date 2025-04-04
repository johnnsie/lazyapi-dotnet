
using System.Text.Json.Serialization;

namespace LazyAPI.Models;

public class NameInfo
{
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }
    [JsonPropertyName("birthdate")]
    public required string Birthdate { get; set; }
}
