using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace AIService.Infrastructure.ExternalApis;

public class AnthropicApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AnthropicApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        
        var apiKey = _configuration["AnthropicApi:ApiKey"];
        if (!string.IsNullOrEmpty(apiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    }

    public async Task<string> SendMessageAsync(
        string prompt,
        CancellationToken ct = default)
    {
        var model = _configuration["AnthropicApi:Model"] ?? "claude-3-sonnet-20240229";
        
        var request = new
        {
            model = model,
            max_tokens = 2000,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            "https://api.anthropic.com/v1/messages",
            request,
            ct);

        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AnthropicResponse>(cancellationToken: ct);
        return result?.Content?.FirstOrDefault()?.Text ?? "No response";
    }

    // Response models
    public class AnthropicResponse
    {
        [JsonPropertyName("content")]
        public List<ContentBlock>? Content { get; set; }
    }

    public class ContentBlock
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
