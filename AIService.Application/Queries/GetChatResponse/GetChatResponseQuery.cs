using MediatR;

namespace AIService.Application.Queries.GetChatResponse;

public record GetChatResponseQuery(string Message) : IRequest<string>;
