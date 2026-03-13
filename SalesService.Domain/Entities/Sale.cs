namespace SalesService.Domain.Entities;

public class Sale
{
    public int Id { get; set; }
    public int FarmId { get; set; }
    public long? AnimalId { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }

    public Sale()
    {
    }

    public Sale(int farmId, long? animalId, string buyerName, DateTime saleDate, decimal amount, string? notes = null)
    {
        if (farmId <= 0) throw new ArgumentException("FarmId must be greater than zero");
        if (string.IsNullOrWhiteSpace(buyerName)) throw new ArgumentException("BuyerName is required");
        if (amount <= 0) throw new ArgumentException("Amount must be greater than zero");
        if (saleDate > DateTime.UtcNow) throw new ArgumentException("SaleDate cannot be in the future");

        FarmId = farmId;
        AnimalId = animalId;
        BuyerName = buyerName;
        SaleDate = saleDate;
        Amount = amount;
        Notes = notes;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
}
