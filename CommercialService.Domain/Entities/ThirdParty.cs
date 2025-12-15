using System.Collections.Generic;

namespace CommercialService.Domain.Entities;

public class ThirdParty
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string IdentityDocument { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public bool IsSupplier { get; set; }
    public bool IsCustomer { get; set; }
    public bool IsEmployee { get; set; }
    public bool IsVeterinarian { get; set; }

    public string? Address { get; set; }
}
