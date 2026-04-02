using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;
using FluentAssertions;
using InventoryService.Presentation.Middlewares;

namespace BioTechBackend.Tests.Integration
{
    public class GatewayAuthenticationMiddlewareTests
    {
        private Mock<RequestDelegate> _nextMock;
        private Mock<ILogger<GatewayAuthenticationMiddleware>> _loggerMock;
        private IConfiguration _configuration = null!;
        private GatewayAuthenticationMiddleware _middleware = null!;

        public GatewayAuthenticationMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<GatewayAuthenticationMiddleware>>();
        }

        private void SetupMiddleware(Dictionary<string, string?> configValues)
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            _middleware = new GatewayAuthenticationMiddleware(
                _nextMock.Object,
                _configuration,
                _loggerMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_HealthCheckPath_ShouldCallNext()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/health";
            SetupMiddleware(new Dictionary<string, string?>());

            await _middleware.InvokeAsync(context);

            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ValidSecret_NoIPWhitelist_ShouldAllowRequest()
        {
            SetupMiddleware(new Dictionary<string, string?> {
                {"Gateway:Secret", "correct-secret"}
            });
            
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/v1/products";
            context.Request.Headers["X-Gateway-Secret"] = new StringValues("correct-secret");

            await _middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().NotBe(401);
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_InvalidSecret_ShouldReturn401()
        {
            SetupMiddleware(new Dictionary<string, string?> {
                {"Gateway:Secret", "correct-secret"}
            });
            
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/v1/products";
            context.Request.Headers["X-Gateway-Secret"] = new StringValues("wrong-secret");

            await _middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(401);
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
        }

        [Fact]
        public async Task InvokeAsync_ValidSecret_IPNotInWhitelist_ShouldReturn401()
        {
            SetupMiddleware(new Dictionary<string, string?> {
                {"Gateway:Secret", "correct-secret"},
                {"Gateway:AllowedIPs", "10.0.0.5, 172.16.0.10"}
            });
            
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/v1/products";
            context.Request.Headers["X-Gateway-Secret"] = new StringValues("correct-secret");
            context.Connection.RemoteIpAddress = IPAddress.Parse("192.168.1.1");

            await _middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(401);
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
        }

        [Fact]
        public async Task InvokeAsync_ValidSecret_IPInWhitelist_ShouldAllowRequest()
        {
            SetupMiddleware(new Dictionary<string, string?> {
                {"Gateway:Secret", "correct-secret"},
                {"Gateway:AllowedIPs", "10.0.0.5, 172.16.0.10"}
            });
            
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/v1/products";
            context.Request.Headers["X-Gateway-Secret"] = new StringValues("correct-secret");
            context.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.5");

            await _middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().NotBe(401);
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ValidSecret_IPInXForwardedFor_ShouldAllowRequest()
        {
            SetupMiddleware(new Dictionary<string, string?> {
                {"Gateway:Secret", "correct-secret"},
                {"Gateway:AllowedIPs", "10.0.0.5, 172.16.0.10"}
            });
            
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/v1/products";
            context.Request.Headers["X-Gateway-Secret"] = new StringValues("correct-secret");
            context.Request.Headers["X-Forwarded-For"] = new StringValues("172.16.0.10, 192.168.1.1");
            context.Connection.RemoteIpAddress = IPAddress.Parse("10.42.0.1"); 

            await _middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().NotBe(401);
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }
    }
}
