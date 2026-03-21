using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SalesService.Application.Interfaces;
using Xunit;

namespace SalesService.Tests.Integration;

public class DependencyInjectionTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DependencyInjectionTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void ISaleRepository_ShouldBeRegistered()
    {
        using var scope = _factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetService<ISaleRepository>();
        
        Assert.NotNull(repository);
    }
}
