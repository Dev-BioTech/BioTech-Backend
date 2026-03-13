using System.Net.Http.Json;
using Xunit;
using System.Net;
using FluentAssertions;

namespace HerdService.Tests.Integration;

public class BreedsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public BreedsEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Mock dependencies for testing
                var breedRepositoryMock = new Mock<IBreedRepository>();
                breedRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Breed>
                    {
                        new Breed("Holstein"),
                        new Breed("Jersey")
                    });
                
                services.AddScoped<IBreedRepository>(_ => breedRepositoryMock.Object);
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetBreeds_ShouldReturnOkWithBreeds()
    {
        // Arrange
        // Mock JWT authentication
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.GetAsync("/api/v1/breeds");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var breeds = await response.Content.ReadFromJsonAsync<List<BreedResponse>>();
        breeds.Should().NotBeNull();
        breeds.Should().HaveCount(2);
        breeds.Should().Contain(b => b.Name == "Holstein");
        breeds.Should().Contain(b => b.Name == "Jersey");
    }

    [Fact]
    public async Task CreateBreed_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");
        
        var newBreed = new CreateBreedCommand("Angus");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/breeds", newBreed);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var createdBreed = await response.Content.ReadFromJsonAsync<BreedResponse>();
        createdBreed.Should().NotBeNull();
        createdBreed.Name.Should().Be("Angus");
    }

    [Fact]
    public async Task CreateBreed_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Arrange - No auth header
        var newBreed = new CreateBreedCommand("Angus");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/breeds", newBreed);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
