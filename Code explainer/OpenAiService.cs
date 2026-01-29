using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Code_explainer;

/// <summary>
/// OpenAiService hanterar API access. ApiKey finns i appsettings.json
/// </summary>

public class OpenAiService
{
    private readonly HttpClient _http;

    public OpenAiService(string apiKey)
    {
        _http = new HttpClient();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<string> ExplainCodeAsync(string prompt)
    {
        var body = new
        {
            model = "gpt-5-mini",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var response = await _http.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        );

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }
}