namespace HealthService.Domain.Entities;

public class WithdrawalPeriod
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public long AnimalId { get; set; }
    public DateTime StartDate { get; set; }
    public int WithdrawalDays { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ProductType { get; set; } // MILK, MEAT, BOTH
    public string? Reason { get; set; }
    public bool Active { get; set; } = true;

    public void CalculateEndDate()
    {
        EndDate = StartDate.AddDays(WithdrawalDays);
    }
}
