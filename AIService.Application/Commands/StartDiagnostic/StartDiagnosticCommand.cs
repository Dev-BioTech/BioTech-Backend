using AIService.Application.DTOs;
using MediatR;

namespace AIService.Application.Commands.StartDiagnostic;

public record StartDiagnosticCommand(StartDiagnosticRequest Request) : IRequest<DiagnosticResponse>;
