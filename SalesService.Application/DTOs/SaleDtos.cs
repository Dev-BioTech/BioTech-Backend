namespace SalesService.Application.DTOs;

public record SaleDto(
    int Id,
    int FarmId,
    long? AnimalId,
    string BuyerName,
    DateTime SaleDate,
    decimal Amount,
    string? Notes,
    DateTime CreatedAt
);

public record CreateSaleDto(
    int FarmId,
    long? AnimalId,
    string BuyerName,
    DateTime SaleDate,
    decimal Amount,
    string? Notes = null
);

public record UpdateSaleDto(
    string BuyerName,
    DateTime SaleDate,
    decimal Amount
);
