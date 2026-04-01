using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.Infrastructure.Interfaces;

namespace Shared.Infrastructure.Services;

public class HttpMessenger : IMessenger
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpMessenger(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    private void ApplySecurityHeaders(HttpClient client)
    {
        // 1. Apply Gateway Secret (System identification)
        var secret = _configuration["Gateway:Secret"];
        if (!string.IsNullOrEmpty(secret) && !client.DefaultRequestHeaders.Contains("X-Gateway-Secret"))
        {
            client.DefaultRequestHeaders.Add("X-Gateway-Secret", secret);
        }

        // 2. Propagate Identity Headers (User identification) from current request
        var currentContext = _httpContextAccessor.HttpContext;
        if (currentContext != null)
        {
            var headersToPropagate = new[] { "X-User-Id", "X-User-Roles", "X-User-Email", "X-User-Name", "X-Farm-Id" };
            foreach (var header in headersToPropagate)
            {
                if (currentContext.Request.Headers.TryGetValue(header, out var val) && !client.DefaultRequestHeaders.Contains(header))
                {
                    client.DefaultRequestHeaders.Add(header, val.ToString());
                }
            }
        }
    }

    public async Task<TResponse?> GetAsync<TResponse>(string serviceName, string endpoint, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient(serviceName);
        ApplySecurityHeaders(client);
        
        var response = await client.GetAsync(endpoint, ct);
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return default;

        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: ct);
    }

    public async Task PostAsync<TRequest>(string serviceName, string endpoint, TRequest request, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient(serviceName);
        ApplySecurityHeaders(client);
        
        var response = await client.PostAsJsonAsync(endpoint, request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string serviceName, string endpoint, TRequest request, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient(serviceName);
        ApplySecurityHeaders(client);
        
        var response = await client.PostAsJsonAsync(endpoint, request, ct);
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return default;

        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: ct);
    }
}
