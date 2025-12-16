namespace HealthService.Application.Events;

public record AnimalCreated(long AnimalId, int FarmId, DateTime BirthDate);

public record HealthEventRegistered(long HealthEventId, int FarmId, string EventType, DateTime EventDate);

public record QuarantineStarted(long AnimalId, int FarmId, DateTime StartDate, string Reason);

public record QuarantineEnded(long AnimalId, int FarmId, DateTime EndDate);
