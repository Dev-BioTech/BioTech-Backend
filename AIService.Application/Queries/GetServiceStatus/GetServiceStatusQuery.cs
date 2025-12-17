using AIService.Application.DTOs;
using MediatR;

namespace AIService.Application.Queries.GetServiceStatus;

public record GetServiceStatusQuery() : IRequest<ServiceStatusResponse>;

public record ServiceStatusResponse(
    Dictionary<string, ServiceHealthStatus> Services,
    DateTime CheckedAt
);
