using System.Net.Http.Json;
using Xunit;
using System.Net;
using FluentAssertions;

namespace ReproductionService.Tests.Integration;

public class ReproductionEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ReproductionEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Mock repositories
                var pregnancyRepositoryMock = new Mock<IPregnancyRepository>();
                pregnancyRepositoryMock.Setup(x => x.GetByFarmIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Pregnancy>
                    {
                        new Pregnancy(1L, 1, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(100))),
                        new Pregnancy(2L, 1, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(90)))
                    });

                var birthRepositoryMock = new Mock<IBirthRepository>();
                birthRepositoryMock.Setup(x => x.GetByFarmIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Birth>
                    {
                        new Birth(1L, 1, "Calf-001", DateTime.UtcNow.AddDays(-5), 35.5m, "Male"),
                        new Birth(2L, 1, "Calf-002", DateTime.UtcNow.AddDays(-3), 32.0m, "Female")
                    });
                
                services.AddScoped<IPregnancyRepository>(_ => pregnancyRepositoryMock.Object);
                services.AddScoped<IBirthRepository>(_ => birthRepositoryMock.Object);
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetPregnanciesByFarm_ShouldReturnPregnancies()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.GetAsync("/api/v1/Reproduction/pregnancies/farm/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var pregnancies = await response.Content.ReadFromJsonAsync<List<PregnancyDto>>();
        pregnancies.Should().NotBeNull();
        pregnancies.Should().HaveCount(2);
        pregnancies.Should().OnlyContain(p => p.AnimalId > 0);
    }

    [Fact]
    public async Task GetBirthsByFarm_ShouldReturnBirths()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.GetAsync("/api/v1/Reproduction/births/farm/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var births = await response.Content.ReadFromJsonAsync<List<BirthDto>>();
        births.Should().NotBeNull();
        births.Should().HaveCount(2);
        births.Should().OnlyContain(b => b.Weight > 0);
    }

    [Fact]
    public async Task RegisterBirth_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");
        
        var birthData = new RegisterBirthCommand(1L, "Calf-003", 38.0m, "Male", DateTime.UtcNow.AddDays(-1));

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Reproduction/births", birthData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var createdBirth = await response.Content.ReadFromJsonAsync<BirthDto>();
        createdBirth.Should().NotBeNull();
        createdBirth.OffspringTag.Should().Be("Calf-003");
        createdBirth.Weight.Should().Be(38.0m);
        createdBirth.Gender.Should().Be("Male");
    }

    [Fact]
    public async Task RegisterBirth_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Arrange - No auth header
        var birthData = new RegisterBirthCommand(1L, "Calf-003", 38.0m, "Male", DateTime.UtcNow.AddDays(-1));

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Reproduction/births", birthData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
