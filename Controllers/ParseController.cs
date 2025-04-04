using Microsoft.AspNetCore.Mvc;
using LazyAPI.Models;
using System.Text;
using System.Text.Json;

namespace LazyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParseController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ParseController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] JsonElement rawInput)
    {
        var promptText = rawInput.ToString();

        var ollamaRequest = new OllamaRequest
        {
            model = "prod-brain4", 
            prompt = promptText,
            stream = false
        };

        var content = new StringContent(JsonSerializer.Serialize(ollamaRequest), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error from Ollama");

        var responseBody = await response.Content.ReadAsStringAsync();
        
        try
        {
            // Step 1: Deserialize the outer Ollama response
            var ollamaResponse = JsonSerializer.Deserialize<OllamaWrapperResponse>(responseBody);
            var rawInner = ollamaResponse?.response?.Trim();
            string cleaned = StringHelpers.RemoveBackslashes(rawInner);
            Console.WriteLine(cleaned);

            int start = cleaned.IndexOf('{');
            int end = cleaned.LastIndexOf('}');
            string jsonOnly = cleaned.Substring(start, end - start + 1);
            
            var nameInfo = JsonSerializer.Deserialize<NameInfo>(jsonOnly);

            Console.WriteLine($"First Name: {nameInfo.FirstName}");
            Console.WriteLine($"Last Name: {nameInfo.LastName}");
            Console.WriteLine($"Birthdate: {nameInfo.Birthdate}");

            return Ok(nameInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Parsing error: " + ex.Message);
            return BadRequest("Failed to parse name info.");
        }

    }
}

public static class StringHelpers
{
    public static string RemoveBackslashes(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return input.Replace("\\", "");
    }
}
