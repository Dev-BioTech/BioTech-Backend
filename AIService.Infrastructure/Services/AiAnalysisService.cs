using System.Text.Json;
using AIService.Application.DTOs;
using AIService.Application.Interfaces;
using AIService.Infrastructure.ExternalApis;

namespace AIService.Infrastructure.Services;

public class AiAnalysisService : IAiAnalysisService
{
    private readonly AnthropicApiClient _anthropicClient;

    public AiAnalysisService(AnthropicApiClient anthropicClient)
    {
        _anthropicClient = anthropicClient;
    }

    public async Task<AiDiagnosticResult> AnalyzeError502Async(
        string serviceName,
        string logSnapshot,
        ServiceHealthStatus healthStatus,
        DockerInspectionResult dockerInfo,
        CancellationToken ct = default)
    {
        var prompt = BuildAnalysisPrompt(serviceName, logSnapshot, healthStatus, dockerInfo);
        var aiResponseText = await _anthropicClient.SendMessageAsync(prompt, ct);
        
        // Parse the JSON response from AI
        // We expect the AI to return valid JSON as per the prompt instructions.
        try
        {
             // Attempt to find JSON block if wrapped in markdown
             var jsonContent = ExtractJson(aiResponseText);
             var analysis = JsonSerializer.Deserialize<AiAnalysisDto>(jsonContent, new JsonSerializerOptions
             {
                 PropertyNameCaseInsensitive = true
             });

             if (analysis == null) throw new Exception("Failed to parse AI response");

             return new AiDiagnosticResult(
                 RootCause: analysis.RootCause ?? "Unknown",
                 DetailedAnalysis: analysis.DetailedAnalysis ?? aiResponseText,
                 PossibleCauses: analysis.PossibleCauses ?? new List<string>(),
                 Recommendations: analysis.Recommendations ?? new List<string>(),
                 ConfidenceScore: analysis.ConfidenceScore
             );
        }
        catch (Exception)
        {
            // Fallback if JSON parsing fails
            return new AiDiagnosticResult(
                RootCause: "AI Analysis Parse Failure",
                DetailedAnalysis: aiResponseText,
                PossibleCauses: new List<string>(),
                Recommendations: new List<string>(),
                ConfidenceScore: 0
            );
        }
    }

    public async Task<List<string>> GenerateRecommendationsAsync(
        string rootCause,
        string serviceName,
        CancellationToken ct = default)
    {
        var prompt = $"Given the root cause '{rootCause}' for service '{serviceName}', list 3 specific technical recommendations to fix it. Return only a JSON array of strings.";
        var response = await _anthropicClient.SendMessageAsync(prompt, ct);
        try 
        {
            var json = ExtractJson(response);
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string> { "Check logs", "Restart service" };
        }
    }

    private string BuildAnalysisPrompt(
        string serviceName,
        string logs,
        ServiceHealthStatus health,
        DockerInspectionResult docker)
    {
        return $@"
You are an expert DevOps engineer analyzing a 502 Bad Gateway error in a .NET microservices system.

**Service:** {serviceName}
**Error:** 502 Bad Gateway

**Health Check Status:**
- Healthy: {health.IsHealthy}
- Response Time: {health.ResponseTimeMs}ms
- Error: {health.ErrorMessage ?? "None"}

**Docker Status:**
- Container Running: {docker.IsRunning}
- CPU Usage: {docker.CpuUsagePercent}%
- Memory Usage: {docker.MemoryUsageMb}MB / {docker.MemoryLimitMb}MB
- Network: {docker.NetworkStatus}

**Recent Logs (last 100 lines):**
```
{logs}
```

**Task:**
1. Identify the ROOT CAUSE of the 502 error
2. Explain WHY this causes a 502 (not 500 or other error)
3. List 3-5 possible causes in order of likelihood
4. Provide specific, actionable recommendations to fix it
5. Rate your confidence (0-100)

**Format your response as purely JSON (no markdown wrapping if possible, or inside ```json block):**
{{
  ""rootCause"": ""brief description"",
  ""detailedAnalysis"": ""detailed explanation"",
  ""possibleCauses"": [""cause1"", ""cause2"", ""cause3""],
  ""recommendations"": [""action1"", ""action2"", ""action3""],
  ""confidenceScore"": 85
}}
";
    }

    private string ExtractJson(string text)
    {
        if (string.IsNullOrEmpty(text)) return "{}";
        
        var startIndex = text.IndexOf("{");
        var endIndex = text.LastIndexOf("}");
        
        if (startIndex >= 0 && endIndex > startIndex)
        {
            return text.Substring(startIndex, endIndex - startIndex + 1);
        }
        return text;
    }

    // DTO for internal deserialization
    private class AiAnalysisDto
    {
        public string? RootCause { get; set; }
        public string? DetailedAnalysis { get; set; }
        public List<string>? PossibleCauses { get; set; }
        public List<string>? Recommendations { get; set; }
        public int ConfidenceScore { get; set; }
    }
}
