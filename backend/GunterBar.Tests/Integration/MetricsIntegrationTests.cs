using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using GunterBar.Presentation;
using GunterBar.Presentation.Extensions;
using System.Net.Http.Json;
using GunterBar.Presentation.Infrastructure;

namespace GunterBar.Tests.Integration;

public class MetricsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IMetricCollector _metrics;

    public MetricsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddCustomMetrics();
                services.AddSingleton<IMetricsConfiguration, TestMetricsConfiguration>();
            });
        });
        
        _metrics = _factory.Services.GetRequiredService<IMetricCollector>();
    }

    [Fact]
    public async Task MetricsEndpoint_ShouldReturn200()
    {
        // Arrange
        using var client = _factory.CreateClient();
        
        // Generate some metrics
        _metrics.OrderCreated("completed");
        _metrics.DrinkAdded("cocktail");
        _metrics.UpdateActiveUsers(1);
        _metrics.RecordCartValue(25.5m);
        
        // Act
        var response = await client.GetAsync("/metrics");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("gunterbar_orders_total", content);
        Assert.Contains("gunterbar_drinks_added_total", content);
        Assert.Contains("gunterbar_active_users", content);
        Assert.Contains("gunterbar_cart_value_dollars", content);
    }

    [Fact]
    public async Task ApiEndpoints_ShouldRecordLatency()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        await client.GetAsync("/api/drinks");
        await client.GetAsync("/api/orders");

        // Verify metrics
        var metricsResponse = await client.GetAsync("/metrics");
        var content = await metricsResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("gunterbar_api_latency_milliseconds", content);
        Assert.Contains("endpoint=\"/api/drinks\"", content);
        Assert.Contains("endpoint=\"/api/orders\"", content);
    }

    [Fact]
    public async Task Login_ShouldRecordAttempts()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act - Simulate login attempts
        var loginData = new { username = "test", password = "test" };
        await client.PostAsJsonAsync("/api/auth/login", loginData);

        // Verify metrics
        var metricsResponse = await client.GetAsync("/metrics");
        var content = await metricsResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("gunterbar_login_attempts_total", content);
    }

    [Fact]
    public async Task Errors_ShouldBeRecorded()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act - Trigger different types of errors
        await client.GetAsync("/api/nonexistent");
        await client.PostAsJsonAsync("/api/drinks", new { }); // Bad request
        await client.DeleteAsync("/api/orders/999"); // Not found
        
        // Verify metrics
        var metricsResponse = await client.GetAsync("/metrics");
        var content = await metricsResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("gunterbar_errors_total", content);
        Assert.Contains("type=\"not_found\"", content);
        Assert.Contains("type=\"bad_request\"", content);
    }

    [Fact]
    public async Task OrderMetrics_ShouldTrackDifferentStatuses()
    {
        // Arrange
        using var client = _factory.CreateClient();
        
        // Act - Record orders with different statuses
        _metrics.OrderCreated("pending");
        _metrics.OrderCreated("completed");
        _metrics.OrderCreated("cancelled");
        
        // Verify metrics
        var response = await client.GetAsync("/metrics");
        var content = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Contains("gunterbar_orders_total{status=\"pending\"}", content);
        Assert.Contains("gunterbar_orders_total{status=\"completed\"}", content);
        Assert.Contains("gunterbar_orders_total{status=\"cancelled\"}", content);
    }

    [Fact]
    public async Task DrinkMetrics_ShouldTrackCategories()
    {
        // Arrange
        using var client = _factory.CreateClient();
        
        // Act - Record drinks from different categories
        _metrics.DrinkAdded("cocktail");
        _metrics.DrinkAdded("beer");
        _metrics.DrinkAdded("wine");
        _metrics.DrinkAdded("non_alcoholic");
        
        // Verify metrics
        var response = await client.GetAsync("/metrics");
        var content = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Contains("gunterbar_drinks_added_total{category=\"cocktail\"}", content);
        Assert.Contains("gunterbar_drinks_added_total{category=\"beer\"}", content);
        Assert.Contains("gunterbar_drinks_added_total{category=\"wine\"}", content);
        Assert.Contains("gunterbar_drinks_added_total{category=\"non_alcoholic\"}", content);
    }

    [Fact]
    public async Task CartValueMetrics_ShouldHandleEdgeCases()
    {
        // Arrange
        using var client = _factory.CreateClient();
        
        // Act - Record different cart values including edge cases
        _metrics.RecordCartValue(0m);
        _metrics.RecordCartValue(999.99m);
        _metrics.RecordCartValue(1500m); // Valor que garantiza que se use el bucket de 1000.0
        _metrics.RecordCartValue(0.01m);
        
        // Verify metrics
        var response = await client.GetAsync("/metrics");
        var content = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Contains("gunterbar_cart_value_dollars", content);
        // Verify that the histogram buckets contain our values
        Assert.Contains("le=\"1000.0\"", content);
        Assert.Contains("le=\"0.1\"", content);
    }

    [Fact]
    public async Task ActiveUsersMetric_ShouldTrackChanges()
    {
        // Arrange
        using var client = _factory.CreateClient();
        
        // Act - Simulate user activity
        _metrics.UpdateActiveUsers(1);  // First user
        _metrics.UpdateActiveUsers(2);  // Second user
        _metrics.UpdateActiveUsers(1);  // One user leaves
        
        // Verify metrics
        var response = await client.GetAsync("/metrics");
        var content = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Contains("gunterbar_active_users", content);
        Assert.Contains("1.0", content); // Current value should be 1
    }
}
