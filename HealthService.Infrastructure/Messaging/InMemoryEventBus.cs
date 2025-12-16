using HealthService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HealthService.Infrastructure.Messaging;

public class InMemoryEventBus : IEventBus
{
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        // In a real scenario, this would publish to RabbitMQ/Kafka.
        // For now, we verify the requirement by logging the emission.
        _logger.LogInformation("Publishing Event: {EventName} - {@Event}", typeof(T).Name, @event);
        return Task.CompletedTask;
    }
}
