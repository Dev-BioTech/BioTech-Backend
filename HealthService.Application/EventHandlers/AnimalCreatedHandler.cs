using MediatR;
using HealthService.Application.Events;
using Microsoft.Extensions.Logging;

namespace HealthService.Application.EventHandlers;

// This handler would be triggered by an infrastructure listener (e.g., RabbitMQ consumer)
// delegating to MediatR, or directly by the bus mock.
// For now, it implements INotificationHandler if we treat events as MediatR notifications,
// or we just define it as a service that 'would' be called.
// To make it functional in this Mock setup, we'll assume the EventBus might eventually publish to MediatR 
// or we just define the logic here for the Consumer.

public class AnimalCreatedHandler : INotificationHandler<INotificationWrapper<AnimalCreated>>
{
    private readonly ILogger<AnimalCreatedHandler> _logger;

    public AnimalCreatedHandler(ILogger<AnimalCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(INotificationWrapper<AnimalCreated> notification, CancellationToken cancellationToken)
    {
        var @event = notification.Event;
        _logger.LogInformation("HealthService: Received AnimalCreated event for Animal {AnimalId} in Farm {FarmId}. Initializing health record check...", @event.AnimalId, @event.FarmId);
        
        // Logic to create initial health record if needed
        return Task.CompletedTask;
    }
}

// Wrapper to make generic events compatible with MediatR if needed, 
// or we can just make the events implement INotification.
public interface INotificationWrapper<T> : INotification
{
    T Event { get; }
}

public class NotificationWrapper<T> : INotificationWrapper<T>
{
    public T Event { get; }
    public NotificationWrapper(T @event) => Event = @event;
}
