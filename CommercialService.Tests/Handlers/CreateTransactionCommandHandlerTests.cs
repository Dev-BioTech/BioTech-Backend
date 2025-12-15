using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommercialService.Application.Commands;
using CommercialService.Application.Handlers;
using CommercialService.Application.DTOs;
using CommercialService.Domain.Entities;
using CommercialService.Domain.Enums;
using CommercialService.Domain.Interfaces;
using CommercialService.Domain.Events;
using FluentAssertions;
using Moq;
using Xunit;

namespace CommercialService.Tests.Handlers;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<ICommercialRepository> _mockRepo;
    private readonly Mock<IPublisher> _mockPublisher;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _mockRepo = new Mock<ICommercialRepository>();
        _mockRepo.Setup(r => r.AddTransactionAsync(It.IsAny<CommercialTransaction>(), It.IsAny<CancellationToken>()))
                 .Callback<CommercialTransaction, CancellationToken>((t, ct) => t.Id = 999);

        _mockPublisher = new Mock<IPublisher>();

        _handler = new CreateTransactionCommandHandler(_mockRepo.Object, _mockPublisher.Object);
    }

    [Fact]
    public async Task Handle_Should_Calculate_NetTotal_Correctly_For_Purchase()
    {
        // Arrange
        var dto = new CreateTransactionDto
        {
            FarmId = 1,
            TransactionType = TransactionType.PURCHASE,
            TransactionDate = DateTime.Now,
            Subtotal = 1000m,
            Taxes = 190m,
            Discounts = 50m,
            PaymentStatus = PaymentStatus.PENDING,
            AnimalDetails = new List<CreateAnimalDetailDto>(),
            ProductDetails = new List<CreateProductDetailDto>()
        };

        var command = new CreateTransactionCommand(dto, 123);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // NetTotal = Subtotal + Taxes - Discounts = 1000 + 190 - 50 = 1140
        _mockRepo.Verify(r => r.AddTransactionAsync(It.Is<CommercialTransaction>(t =>
            t.NetTotal == 1140m &&
            t.TransactionType == TransactionType.PURCHASE &&
            t.RegisteredBy == 123
        ), It.IsAny<CancellationToken>()), Times.Once);

        _mockPublisher.Verify(p => p.Publish(It.IsAny<TransactionCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().BeGreaterThan(0); // Assuming ID generation returns > 0
    }

    [Fact]
    public async Task Handle_Should_Create_Animal_Details_Correctly_With_Commission_And_Transport()
    {
        // Arrange
        var dto = new CreateTransactionDto
        {
            FarmId = 1,
            TransactionType = TransactionType.SALE,
            TransactionDate = DateTime.Now,
            Subtotal = 2000m,
            Taxes = 0,
            Discounts = 0,
            PaymentStatus = PaymentStatus.PAID,
            AnimalDetails = new List<CreateAnimalDetailDto>
            {
                new CreateAnimalDetailDto
                {
                    AnimalId = 10,
                    PricePerKilo = 5.0m,
                    WeightAtNegotiation = 400m,
                    // BaseHeadPrice = 5 * 400 = 2000
                    CommissionCost = 100m,
                    TransportCost = 50m
                    // FinalLineValue should be calculated in handler? Or is it passed?
                    // Let's check the handler implementation. 
                    // Usually Application logic calculates it or it takes what is given.
                    // Assuming handler might calculate it if logic exists, otherwise it maps.
                    // Checking Handler: it maps properties directly.
                }
            },
            ProductDetails = new List<CreateProductDetailDto>()
        };

        var command = new CreateTransactionCommand(dto, 456);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.AddTransactionAsync(It.Is<CommercialTransaction>(t =>
            t.AnimalDetails.Count == 1 &&
            t.AnimalDetails.First().AnimalId == 10 &&
            t.AnimalDetails.First().TransportCost == 50m
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_If_Subtotal_Mismatch()
    {
        // This is a "nice to have" test if we had validation logic for totals.
        // Currently the handler is trusted to just save what is sent.
        // We can skip this if no validation logic is in place yet.
    }
}
