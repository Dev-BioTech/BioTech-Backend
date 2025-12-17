using AIService.Application.DTOs;
using MediatR;

namespace AIService.Application.Queries.Analyze502Error;

public record Analyze502ErrorQuery(Analyze502Request Request) : IRequest<Error502AnalysisResponse>;
