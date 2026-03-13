namespace InventoryService.Application.DTOs;

public record LowStockProductDto(
    int Id,
    string Name,
    decimal CurrentQuantity,
    decimal MinimumStock
);
