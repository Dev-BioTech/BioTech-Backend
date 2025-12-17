using System.Diagnostics;
using System.Text.RegularExpressions;
using AIService.Application.DTOs;
using AIService.Application.Interfaces;

namespace AIService.Infrastructure.Services;

public class LogAnalyzerService : ILogAnalyzerService
{
    public async Task<LogAnalysisResult> AnalyzeLogsAsync(
        string serviceName,
        int lineCount = 100,
        CancellationToken ct = default)
    {
        // Use Docker API to fetch logs
        var containerName = GetContainerName(serviceName);
        var logs = await FetchDockerLogsAsync(containerName, lineCount, ct);
        
        var errors = ExtractErrors(logs);
        var warnings = ExtractWarnings(logs);
        var exceptions = ExtractExceptions(logs);
        
        return new LogAnalysisResult(
            TotalLines: logs.Length,
            ErrorCount: errors.Count,
            WarningCount: warnings.Count,
            RecentErrors: errors.Take(10).ToList(),
            RecentExceptions: exceptions.Take(5).ToList(),
            LogSnapshot: logs
        );
    }
    
    private string GetContainerName(string serviceName)
    {
        // Simple mapping based on project conventions
        // Assumes container names like feeding-api, herd-api etc.
        return serviceName.ToLower().Replace("service", "-api");
    }

    private async Task<string> FetchDockerLogsAsync(
        string containerName,
        int lines,
        CancellationToken ct)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"logs --tail {lines} {containerName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Capture stderr too as docker logs often go there
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync(ct);
        
        return string.IsNullOrEmpty(output) ? error : output;
    }

    private List<string> ExtractErrors(string logs)
    {
        // Simple case-insensitive search for "error"
        if (string.IsNullOrEmpty(logs)) return new List<string>();
        
        return logs.Split('\n')
            .Where(l => l.Contains("error", StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private List<string> ExtractWarnings(string logs)
    {
        if (string.IsNullOrEmpty(logs)) return new List<string>();

        return logs.Split('\n')
            .Where(l => l.Contains("warn", StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private List<string> ExtractExceptions(string logs)
    {
        if (string.IsNullOrEmpty(logs)) return new List<string>();

        return logs.Split('\n')
            .Where(l => l.Contains("Exception") || l.TrimStart().StartsWith("at "))
            .ToList();
    }
}
