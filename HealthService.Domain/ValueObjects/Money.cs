namespace HealthService.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; private set; }

    private Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative", nameof(amount));
            
        Amount = Math.Round(amount, 2);
    }

    public static Money FromDecimal(decimal amount) => new(amount);
    public static Money Zero() => new(0);

    public static Money operator +(Money left, Money right) 
        => new(left.Amount + right.Amount);

    public static Money operator -(Money left, Money right) 
        => new(left.Amount - right.Amount);

    public static Money operator *(Money money, decimal multiplier) 
        => new(money.Amount * multiplier);

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount;
    }

    public override bool Equals(object? obj) => Equals(obj as Money);
    public override int GetHashCode() => Amount.GetHashCode();
    public override string ToString() => Amount.ToString("F2");
}
