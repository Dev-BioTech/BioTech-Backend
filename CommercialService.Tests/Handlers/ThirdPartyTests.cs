using System;
using System.Threading;
using System.Threading.Tasks;
using CommercialService.Application.Commands;
using CommercialService.Application.DTOs;
using CommercialService.Domain.Entities;
using CommercialService.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CommercialService.Tests.Handlers;

public class ThirdPartyTests
{
    private readonly Mock<IThirdPartyRepository> _mockRepo;
    private readonly CreateThirdPartyCommandHandler _createHandler;

    public ThirdPartyTests()
    {
        _mockRepo = new Mock<IThirdPartyRepository>();
        _createHandler = new CreateThirdPartyCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Create_Should_Throw_Exception_If_Document_Already_Exists()
    {
        // Arrange
        var dto = new CreateThirdPartyDto
        {
            FarmId = 1,
            IdentityDocument = "123456789",
            FullName = "Existing User"
        };

        // Mock that the document DOES exist
        _mockRepo.Setup(r => r.ExistsAsync(1, "123456789", It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

        var command = new CreateThirdPartyCommand(dto);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _createHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Create_Should_Succeed_If_Document_Is_New()
    {
        // Arrange
        var dto = new CreateThirdPartyDto
        {
            FarmId = 1,
            IdentityDocument = "987654321",
            FullName = "New User"
        };

        // Mock that document does NOT exist
        _mockRepo.Setup(r => r.ExistsAsync(1, "987654321", It.IsAny<CancellationToken>()))
                 .ReturnsAsync(false);

        // Setup AddAsync to simulate ID generation
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<ThirdParty>(), It.IsAny<CancellationToken>()))
                 .Callback<ThirdParty, CancellationToken>((tp, ct) => tp.Id = 55);

        var command = new CreateThirdPartyCommand(dto);

        // Act
        var result = await _createHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(55);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<ThirdParty>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
