using System.Net.Http.Json;
using Xunit;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using SalesService.Application.Interfaces;
using SalesService.Application.DTOs;
using SalesService.Domain.Entities;
using Shared.Infrastructure.Common;
using AuthService.Application.Interfaces;

namespace SalesService.Tests.Integration;

public class SalesEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public SalesEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing registration if any
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISaleRepository));
                if (descriptor != null) services.Remove(descriptor);
                
                var userDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICurrentUserService));
                if (userDescriptor != null) services.Remove(userDescriptor);

                // Mock sales repository
                var saleRepositoryMock = new Mock<ISaleRepository>();
                saleRepositoryMock.Setup(x => x.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Sale>
                    {
                        new Sale(1, 1L, "John Doe", DateTime.UtcNow.AddDays(-5), 1500.00m),
                        new Sale(1, 2L, "Jane Smith", DateTime.UtcNow.AddDays(-3), 1200.00m)
                    });
                
                saleRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((int id, CancellationToken _) => new Sale(1, (long)id, "Mock Buyer", DateTime.UtcNow, 1000m) { Id = id });

                saleRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Sale s, CancellationToken _) => { s.Id = 1; return s; });

                saleRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                saleRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                services.AddScoped<ISaleRepository>(_ => saleRepositoryMock.Object);

                // Mock current user service
                var currentUserMock = new Mock<ICurrentUserService>();
                currentUserMock.Setup(x => x.UserId).Returns(1);
                currentUserMock.Setup(x => x.IsAuthenticated).Returns(true);
                services.AddScoped<ICurrentUserService>(_ => currentUserMock.Object);
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetSales_ShouldReturnUserSales()
    {
        var response = await _client.GetAsync("/api/v1/sales");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<SaleDto>>>();
        apiResult.Should().NotBeNull();
        apiResult.Success.Should().BeTrue();
        apiResult.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ShouldReturnSale()
    {
        var response = await _client.GetAsync("/api/v1/sales/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<SaleDto>>();
        apiResult.Should().NotBeNull();
        apiResult.Success.Should().BeTrue();
        apiResult.Data.Id.Should().Be(1);
    }

    [Fact]
    public async Task CreateSale_WithValidData_ShouldReturnCreated()
    {
        var saleData = new CreateSaleDto(1, 3L, "Bob Wilson", DateTime.UtcNow, 1800.00m, "Excellent condition");
        var response = await _client.PostAsJsonAsync("/api/v1/sales", saleData);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<SaleDto>>();
        apiResult.Should().NotBeNull();
        apiResult.Success.Should().BeTrue();
        apiResult.Data.BuyerName.Should().Be("Bob Wilson");
    }

    [Fact]
    public async Task UpdateSale_WithValidData_ShouldReturnOk()
    {
        var updateDto = new UpdateSaleDto("Updated Buyer", DateTime.UtcNow.AddDays(-1), 2000.00m);
        var response = await _client.PutAsJsonAsync("/api/v1/sales/1", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<SaleDto>>();
        apiResult.Should().NotBeNull();
        apiResult.Success.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteSale_WithValidId_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/v1/sales/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        apiResult.Should().NotBeNull();
        apiResult.Success.Should().BeTrue();
    }
}
