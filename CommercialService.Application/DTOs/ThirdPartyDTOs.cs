namespace CommercialService.Application.DTOs;

public class CreateThirdPartyDto
{
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

public class UpdateThirdPartyDto : CreateThirdPartyDto
{
    // Inherits everything, but maybe we don't allow changing FarmId or Identity easily?
    // For simplicity, we reuse the structure but ID comes from route.
}

public class ThirdPartyDto
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
