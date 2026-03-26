using FluentAssertions;
using HerdService.Application.Commands;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using Moq;
using Xunit;

namespace BioTechBackend.Tests.Integration;

public class AnimalDataFlowTests
{
    private readonly Mock<IAnimalRepository> _animalRepositoryMock;
    private readonly List<Animal> _animals;

    public AnimalDataFlowTests()
    {
        _animalRepositoryMock = new Mock<IAnimalRepository>();
        _animals = new List<Animal>();
        SetupMockRepository();
    }

    private void SetupMockRepository()
    {
        _animalRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Animal>(), It.IsAny<CancellationToken>()))
            .Callback<Animal, CancellationToken>((a, ct) => 
            {
                // Simple auto-increment ID
                var nextId = _animals.Count + 1;
                var property = typeof(Animal).GetProperty("Id");
                property?.SetValue(a, (long)nextId);
                _animals.Add(a);
            })
            .ReturnsAsync((Animal a, CancellationToken ct) => a);

        _animalRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((long id, CancellationToken ct) => _animals.FirstOrDefault(a => a.Id == id));

        _animalRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Animal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _animalRepositoryMock.Setup(x => x.VisualCodeExistsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
    }

    [Fact]
    public async Task RegisterAnimal_WithWeightAndHeight_ShouldWork()
    {
        // Arrange
        var handler = new RegisterAnimalCommandHandler(_animalRepositoryMock.Object);
        var command = new RegisterAnimalCommand(
            VisualCode: "V-001",
            FarmId: 1,
            Sex: "F",
            BirthDate: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-2)),
            CategoryId: 1,
            BreedId: 1,
            Name: "Test Cow",
            ElectronicCode: "E-001",
            Color: "B&W",
            Purpose: "MEAT",
            Origin: "BIRTH",
            InitialCost: 1000m,
            MotherId: null,
            FatherId: null,
            ExternalMother: null,
            ExternalFather: null,
            BatchId: null,
            PaddockId: null,
            Weight: 450.5m,
            Height: 135.2m,
            UserId: 1
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.VisualCode.Should().Be("V-001");
        result.Weight.Should().Be(450.5m);
        result.Height.Should().Be(135.2m);
        
        // Verify in "database"
        var animalInDb = _animals.FirstOrDefault(a => a.VisualCode == "V-001");
        animalInDb.Should().NotBeNull();
        animalInDb!.Weight.Should().Be(450.5m);
        animalInDb!.Height.Should().Be(135.2m);
    }

    [Fact]
    public async Task UpdateAnimal_WithWeightAndHeight_ShouldWork()
    {
        // Arrange
        var initialAnimal = Animal.Create("V-002", 1, "M", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)), weight: 200m, height: 100m);
        await _animalRepositoryMock.Object.AddAsync(initialAnimal);

        var handler = new UpdateAnimalCommandHandler(_animalRepositoryMock.Object);
        var command = new UpdateAnimalCommand(
            Id: initialAnimal.Id,
            VisualCode: "V-002-U",
            ElectronicCode: initialAnimal.ElectronicCode,
            Name: "Updated Name",
            Color: initialAnimal.Color,
            BreedId: initialAnimal.BreedId,
            CategoryId: initialAnimal.CategoryId,
            Purpose: initialAnimal.Purpose,
            Sex: "M",
            BirthDate: initialAnimal.BirthDate,
            Origin: initialAnimal.Origin,
            EntryDate: initialAnimal.EntryDate,
            InitialCost: initialAnimal.InitialCost,
            MotherId: null,
            FatherId: null,
            ExternalMother: null,
            ExternalFather: null,
            Weight: 250.7m,
            Height: 115.3m,
            UserId: 1
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.VisualCode.Should().Be("V-002-U");
        result.Weight.Should().Be(250.7m);
        result.Height.Should().Be(115.3m);
        
        // Verify in "database"
        var animalInDb = _animals.FirstOrDefault(a => a.Id == initialAnimal.Id);
        animalInDb.Should().NotBeNull();
        animalInDb!.Weight.Should().Be(250.7m);
        animalInDb!.Height.Should().Be(115.3m);
        animalInDb!.VisualCode.Should().Be("V-002-U");
    }

    [Fact]
    public async Task UpdateWeight_Specifically_ShouldWork()
    {
        // Arrange
        var initialAnimal = Animal.Create("V-003", 1, "F", DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6)), weight: 150m, height: 90m);
        await _animalRepositoryMock.Object.AddAsync(initialAnimal);

        var handler = new UpdateAnimalWeightCommandHandler(_animalRepositoryMock.Object);
        var command = new UpdateAnimalWeightCommand(
            AnimalId: initialAnimal.Id,
            NewWeight: 175.5m,
            WeighDate: DateOnly.FromDateTime(DateTime.UtcNow),
            UserId: 1
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Weight.Should().Be(175.5m);
        result.Height.Should().Be(90m); // Height remained same
        
        // Verify in "database"
        initialAnimal.Weight.Should().Be(175.5m);
    }
}
