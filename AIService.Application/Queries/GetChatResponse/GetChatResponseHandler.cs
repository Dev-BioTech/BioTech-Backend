using MediatR;
using AIService.Application.Interfaces;

namespace AIService.Application.Queries.GetChatResponse;

public class GetChatResponseHandler : IRequestHandler<GetChatResponseQuery, string>
{
    private readonly IGeminiService _geminiService;

    public GetChatResponseHandler(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    public async Task<string> Handle(GetChatResponseQuery request, CancellationToken cancellationToken)
    {
        return await _geminiService.GenerateContentAsync(request.Message);
    }
}
