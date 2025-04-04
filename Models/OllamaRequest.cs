
namespace LazyAPI.Models;
public class OllamaRequest
{
    public string model { get; set; } = "mistral";
    public string prompt { get; set; }
    public bool stream { get; set; } = false;
}
