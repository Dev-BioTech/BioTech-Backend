using MediatR;

namespace AIService.Application.Queries.GetChatResponse;

public record GetChatResponseQuery(string Message, int FarmId, int UserId) : IRequest<string>;
