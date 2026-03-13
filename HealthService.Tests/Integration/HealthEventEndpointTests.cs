using System.Net.Http.Json;
using Xunit;
using System.Net;
using FluentAssertions;

namespace HealthService.Tests.Integration;

public class HealthEventEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEventEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Mock health event repository
                var healthEventRepositoryMock = new Mock<IHealthEventRepository>();
                healthEventRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new HealthEvent
                    {
                        Id = 1L,
                        FarmId = 1,
                        AnimalId = 1L,
                        EventType = "Treatment",
                        EventDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                        Treatment = "Antibiotics",
                        VeterinarianName = "Dr. Smith"
                    });

                healthEventRepositoryMock.Setup(x => x.GetUpcomingEventsAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<HealthEvent>
                    {
                        new HealthEvent 
                        { 
                            Id = 1L, 
                            AnimalId = 1L, 
                            EventType = "Vaccination", 
                            EventDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) 
                        },
                        new HealthEvent 
                        { 
                            Id = 2L, 
                            AnimalId = 2L, 
                            EventType = "Check-up", 
                            EventDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)) 
                        }
                    });
                
                services.AddScoped<IHealthEventRepository>(_ => healthEventRepositoryMock.Object);
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task UpdateHealthEvent_WithValidData_ShouldReturnOk()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");
        
        var updateDto = new UpdateHealthEventDto(
            "Updated description",
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            "Updated treatment",
            "Dr. Johnson"
        );

        // Act
        var response = await _client.PutAsJsonAsync("/api/HealthEvent/1", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedEvent = await response.Content.ReadFromJsonAsync<HealthEventResponse>();
        updatedEvent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUpcomingHealthEvents_ShouldReturnUpcomingEvents()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.GetAsync("/api/HealthEvent/upcoming?limit=4");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var upcomingEvents = await response.Content.ReadFromJsonAsync<List<UpcomingHealthEventDto>>();
        upcomingEvents.Should().NotBeNull();
        upcomingEvents.Should().HaveCount(2);
        upcomingEvents.Should().OnlyContain(e => e.ScheduledDate >= DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [Fact]
    public async Task GetUpcomingHealthEvents_WithCustomLimit_ShouldRespectLimit()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.GetAsync("/api/HealthEvent/upcoming?limit=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var upcomingEvents = await response.Content.ReadFromJsonAsync<List<UpcomingHealthEventDto>>();
        upcomingEvents.Should().NotBeNull();
        upcomingEvents.Should().HaveCountLessThanOrEqualTo(1);
    }
}
