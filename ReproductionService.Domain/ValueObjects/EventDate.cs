namespace ReproductionService.Domain.ValueObjects;

public record EventDate
{
    public DateTime Value { get; }

    public EventDate(DateTime value)
    {
        if (value.Date > DateTime.UtcNow.Date)
            throw new ArgumentException("Date cannot be in the future", nameof(value));
        Value = value.Date;
    }

    public static implicit operator DateTime(EventDate date) => date.Value;
    public static implicit operator EventDate(DateTime value) => new(value);
}
