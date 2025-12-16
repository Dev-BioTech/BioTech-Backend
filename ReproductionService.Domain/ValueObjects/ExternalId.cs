namespace ReproductionService.Domain.ValueObjects;

public record ExternalId
{
    public int Value { get; }

    public ExternalId(int value)
    {
        if (value <= 0)
            throw new ArgumentException("ID must be greater than zero", nameof(value));
        Value = value;
    }

    public static implicit operator int(ExternalId id) => id.Value;
    public static implicit operator ExternalId(int value) => new(value);
}
