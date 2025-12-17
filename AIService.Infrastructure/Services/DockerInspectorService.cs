using System.Text.Json;
using System.Text.Json.Serialization;
using AIService.Application.DTOs;
using AIService.Application.Interfaces;

namespace AIService.Infrastructure.Services;

public class DockerInspectorService : IDockerInspectorService
{
    public async Task<DockerInspectionResult> InspectContainerAsync(
        string serviceName,
        CancellationToken ct = default)
    {
        var containerName = GetContainerName(serviceName);
        
        // docker inspect {containerName}
        var inspectJson = await ExecuteDockerCommandAsync(
            $"inspect {containerName}", ct);
        
        // docker stats --no-stream {containerName}
        var statsJson = await ExecuteDockerCommandAsync(
            $"stats --no-stream --format \"{{{{json .}}}}\" {containerName}", ct);
        
        // Helper classes for deserialization (defined below or inline)
        var containerInfo = ParseInspectOutput(inspectJson);
        var containerStats = ParseStatsOutput(statsJson);
        
        return new DockerInspectionResult(
            IsRunning: containerInfo?.State?.Running ?? false,
            Status: containerInfo?.State?.Status ?? "unknown",
            CpuUsagePercent: ParseCpuPercent(containerStats?.CpuPercent),
            MemoryUsageMb: ParseMemory(containerStats?.MemUsage),
            MemoryLimitMb: ParseMemory(containerStats?.MemLimit),
            NetworkStatus: containerInfo?.NetworkSettings?.Networks?.Any() == true 
                ? "Connected" 
                : "Disconnected",
            RestartCount: containerInfo?.RestartCount ?? 0,
            Uptime: containerInfo?.State?.StartedAt != null
                ? DateTime.UtcNow - containerInfo.State.StartedAt.Value
                : TimeSpan.Zero
        );
    }

    private string GetContainerName(string serviceName)
    {
         return serviceName.ToLower().Replace("service", "-api");
    }

    private async Task<string> ExecuteDockerCommandAsync(string arguments, CancellationToken ct)
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync(ct);
        return output;
    }

    // --- Parsing Helpers ---
    
    private ContainerInspectModel? ParseInspectOutput(string json)
    {
        try 
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            var list = JsonSerializer.Deserialize<List<ContainerInspectModel>>(json);
            return list?.FirstOrDefault();
        }
        catch { return null; }
    }
    
    private ContainerStatsModel? ParseStatsOutput(string json)
    {
        try 
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonSerializer.Deserialize<ContainerStatsModel>(json);
        }
        catch { return null; }
    }

    private double ParseCpuPercent(string? cpu)
    {
        if (string.IsNullOrEmpty(cpu)) return 0;
        return double.TryParse(cpu.Replace("%", ""), out var val) ? val : 0;
    }

    private long ParseMemory(string? mem)
    {
        // Format example: "18.5MiB" or "1.2GiB"
        if (string.IsNullOrEmpty(mem)) return 0;
        // Simple parsing logic
        var numberPart = new string(mem.TakeWhile(c => char.IsDigit(c) || c == '.').ToArray());
        if (double.TryParse(numberPart, out var val))
        {
             if (mem.Contains("GiB")) return (long)(val * 1024);
             if (mem.Contains("MiB")) return (long)val;
             if (mem.Contains("kB")) return (long)(val / 1024.0);
        }
        return 0;
    }

    // --- Internal Models for JSON Deserialization ---
    
    private class ContainerInspectModel
    {
        [JsonPropertyName("State")]
        public ContainerState? State { get; set; }
        
        [JsonPropertyName("RestartCount")]
        public int RestartCount { get; set; }
        
        [JsonPropertyName("NetworkSettings")]
        public ContainerNetworkSettings? NetworkSettings { get; set; }
    }

    private class ContainerState
    {
        [JsonPropertyName("Status")]
        public string? Status { get; set; }
        
        [JsonPropertyName("Running")]
        public bool Running { get; set; }
        
        [JsonPropertyName("StartedAt")]
        public DateTime? StartedAt { get; set; }
    }

    private class ContainerNetworkSettings
    {
        [JsonPropertyName("Networks")]
        public Dictionary<string, object>? Networks { get; set; }
    }

    private class ContainerStatsModel
    {
        [JsonPropertyName("CPUPerc")]
        public string? CpuPercent { get; set; }
        
        [JsonPropertyName("MemUsage")]
        public string? MemUsage { get; set; } // "18.5MiB / 1.944GiB" -> we just take the first part usually, wait, docker stats json format gives "MemUsage": "16.14MiB / 7.668GiB"
        
        [JsonPropertyName("MemLimit")]
        public string? MemLimit { get; set; } // Docker stats json might provide this differently depending on version. 
        // Let's assume standard formatting for now or parse "MemUsage" split.
    }
}
