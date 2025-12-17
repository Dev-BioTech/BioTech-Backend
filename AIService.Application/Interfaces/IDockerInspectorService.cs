using AIService.Application.DTOs;

namespace AIService.Application.Interfaces;

public interface IDockerInspectorService
{
    Task<DockerInspectionResult> InspectContainerAsync(string serviceName, CancellationToken ct = default);
}
