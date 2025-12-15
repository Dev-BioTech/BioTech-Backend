namespace CommercialService.Domain.Entities;

public class TransactionProductDetail
{
    public long Id { get; set; }
    public long TransactionId { get; set; }
    public int ProductId { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? LineSubtotal { get; set; }

    public long? InventoryMovementId { get; set; }

    // Navigation Property
    public CommercialTransaction Transaction { get; set; } = null!;
}
