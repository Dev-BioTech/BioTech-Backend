namespace ReproductionService.Domain.ValueObjects;

public record Observation
{
    public string? Value { get; }

    public Observation(string? value)
    {
        if (value != null && value.Length > 2000)
            throw new ArgumentException("Observation cannot exceed 2000 characters", nameof(value));
        Value = value;
    }

    public static implicit operator string?(Observation? observation) => observation?.Value;
    public static implicit operator Observation(string? value) => new(value);
}
