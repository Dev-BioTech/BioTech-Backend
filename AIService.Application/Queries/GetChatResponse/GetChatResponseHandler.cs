using MediatR;
using AIService.Application.Interfaces;
using AIService.Application.DTOs;
using Shared.Infrastructure.Interfaces;
using System.Text;

namespace AIService.Application.Queries.GetChatResponse;

public class GetChatResponseHandler : IRequestHandler<GetChatResponseQuery, string>
{
    private readonly IGeminiService _geminiService;
    private readonly IMessenger _messenger;

    public GetChatResponseHandler(IGeminiService geminiService, IMessenger messenger)
    {
        _geminiService = geminiService;
        _messenger = messenger;
    }

    public async Task<string> Handle(GetChatResponseQuery request, CancellationToken cancellationToken)
    {
        var farmData = await GetFarmDataAsync(request.FarmId, cancellationToken);
        var enrichedPrompt = BuildEnrichedPrompt(request.Message, farmData);
        return await _geminiService.GenerateContentAsync(enrichedPrompt);
    }

    private async Task<FarmDataContext> GetFarmDataAsync(int farmId, CancellationToken ct)
    {
        var farmData = new FarmDataContext();

        try
        {
            var animals = await _messenger.GetAsync<IEnumerable<AnimalSummary>>(
                "HerdService", 
                $"/api/v1/animals?farmId={farmId}&includeInactive=false", 
                ct
            );
            
            if (animals != null)
            {
                farmData.TotalAnimals = animals.Count();
                farmData.AnimalsByCategory = animals
                    .GroupBy(a => a.Category)
                    .ToDictionary(g => g.Key, g => g.Count());
            }

            var healthEvents = await _messenger.GetAsync<IEnumerable<HealthEventSummary>>(
                "HealthService", 
                $"/api/HealthEvent/farm?page=1&pageSize=10", 
                ct
            );
            
            if (healthEvents != null)
            {
                farmData.RecentHealthEvents = healthEvents.Take(5).ToList();
            }

            var feedingEvents = await _messenger.GetAsync<IEnumerable<FeedingEventSummary>>(
                "FeedingService", 
                $"/api/v1/FeedingEvents/farm/{farmId}?page=1&pageSize=5", 
                ct
            );
            
            if (feedingEvents != null)
            {
                farmData.RecentFeedingEvents = feedingEvents.ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching farm data: {ex.Message}");
        }

        return farmData;
    }

    private string BuildEnrichedPrompt(string userMessage, FarmDataContext farmData)
    {
        var contextBuilder = new StringBuilder();
        
        contextBuilder.AppendLine("Act as an expert veterinary assistant specializing in livestock management.");
        contextBuilder.AppendLine("Here is the context of the user's farm:");
        contextBuilder.AppendLine();
        
        contextBuilder.AppendLine($"**Total Animals**: {farmData.TotalAnimals}");
        
        if (farmData.AnimalsByCategory?.Any() == true)
        {
            contextBuilder.AppendLine("**Distribution by Category**:");
            foreach (var category in farmData.AnimalsByCategory)
            {
                contextBuilder.AppendLine($"  - {category.Key}: {category.Value}");
            }
        }
        
        if (farmData.RecentHealthEvents?.Any() == true)
        {
            contextBuilder.AppendLine();
            contextBuilder.AppendLine("**Recent Health Events**:");
            foreach (var evt in farmData.RecentHealthEvents.Take(3))
            {
                contextBuilder.AppendLine($"  - {evt.EventType} on {evt.EventDate:yyyy-MM-dd}");
            }
        }
        
        if (farmData.RecentFeedingEvents?.Any() == true)
        {
            contextBuilder.AppendLine();
            contextBuilder.AppendLine("**Recent Feeding**:");
            var lastFeeding = farmData.RecentFeedingEvents.First();
            contextBuilder.AppendLine($"  - Last feeding: {lastFeeding.SupplyDate:yyyy-MM-dd}");
        }
        
        contextBuilder.AppendLine();
        contextBuilder.AppendLine($"**User Question**: {userMessage}");
        contextBuilder.AppendLine();
        contextBuilder.AppendLine("Respond based on the provided data. If you don't have enough information, clearly indicate it.");
        
        return contextBuilder.ToString();
    }
}
