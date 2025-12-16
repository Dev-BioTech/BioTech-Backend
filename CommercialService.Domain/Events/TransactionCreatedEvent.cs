using MediatR;
using CommercialService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace CommercialService.Domain.Events;

public record TransactionCreatedEvent(
    long TransactionId,
    int FarmId,
    TransactionType Type,
    DateTime TransactionDate,
    List<TransactionItemDto> Items
) : INotification;

public record TransactionItemDto(
    string ItemType, // "Animal" or "Product"
    long EntityId,   // AnimalId or ProductId
    decimal Quantity, // 1 for animals, N for products
    decimal UnitPrice,
    decimal TotalValue
);
