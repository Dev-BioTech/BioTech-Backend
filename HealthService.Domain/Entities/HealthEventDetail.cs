using HealthService.Domain.ValueObjects;

namespace HealthService.Domain.Entities;

public class HealthEventDetail
{
    public long Id { get; set; }
    public long HealthEventId { get; set; }
    public int ProductId { get; set; }
    public decimal DosePerAnimal { get; set; }
    public decimal TotalQuantityDeducted { get; set; }
    
    public Money UnitCostAtMoment { get; set; } = Money.Zero();
    public Money? CalculatedTotalCost { get; private set; }
    
    public string? AdministrationRoute { get; set; } // SUBCUTANEOUS, INTRAMUSCULAR, INTRAVENOUS, ORAL, TOPICAL

    public virtual HealthEvent? HealthEvent { get; set; }

    public void CalculateCost()
    {
        if (UnitCostAtMoment != null)
        {
            CalculatedTotalCost = UnitCostAtMoment * TotalQuantityDeducted;
        }
    }
}
