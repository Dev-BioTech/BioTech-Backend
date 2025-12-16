namespace CommercialService.Domain.Entities;

public class TransactionAnimalDetail
{
    public long Id { get; set; }
    public long TransactionId { get; set; }
    public long AnimalId { get; set; }

    public decimal? PricePerKilo { get; set; }
    public decimal? WeightAtNegotiation { get; set; }
    public decimal BaseHeadPrice { get; set; }

    public decimal CommissionCost { get; set; }
    public decimal TransportCost { get; set; }
    public decimal? FinalLineValue { get; set; }

    public long? AnimalMovementId { get; set; }

    // Navigation Property
    public CommercialTransaction Transaction { get; set; } = null!;
}
