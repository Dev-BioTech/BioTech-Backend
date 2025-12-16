using System;
using System.Collections.Generic;

namespace InventoryService.Application.IntegrationEvents;

// Must match the shape of the event published by CommercialService
public record TransactionCreatedEvent(
    long TransactionId,
    int FarmId,
    string Type, // Changing to string to avoid Enum dependency issues or duplicate Enum
    DateTime TransactionDate,
    List<TransactionItemDto> Items
);

public record TransactionItemDto(
    string ItemType,
    long EntityId,
    decimal Quantity,
    decimal UnitPrice,
    decimal TotalValue
);
