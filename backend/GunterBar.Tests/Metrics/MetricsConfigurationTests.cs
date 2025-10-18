using GunterBar.Presentation.Extensions;
using GunterBar.Presentation.Infrastructure;
using GunterBar.Presentation.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace GunterBar.Tests.Metrics;

public class MetricsConfigurationTests
{
    private readonly TestServer _server;

    public MetricsConfigurationTests()
    {
        var webBuilder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddCustomMetrics();
            })
            .Configure(app =>
            {
                app.UseCustomMetrics();
            });

        _server = new TestServer(webBuilder);
    }

    [Fact]
    public async Task MetricsConfiguration_ShouldRegisterAllServices()
    {
        // Arrange
        var serviceProvider = _server.Services;

        // Assert
        Assert.NotNull(serviceProvider.GetService<IMetricCollector>());
        Assert.NotNull(serviceProvider.GetService<IMetricsConfiguration>());
        Assert.NotNull(serviceProvider.GetService<IMetricsEventListener>());
    }

    [Fact]
    public async Task MetricsEndpoint_ShouldBeConfigured()
    {
        // Arrange
        var client = _server.CreateClient();

        // Act
        var response = await client.GetAsync("/metrics");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public void DefaultConfiguration_ShouldHaveExpectedValues()
    {
        // Arrange
        var config = new DefaultMetricsConfiguration();

        // Assert
        Assert.Equal("/metrics", config.MetricsEndpoint);
        Assert.Contains("gunterbar", config.ApplicationName);
        Assert.NotEmpty(config.CartValueBuckets);
        Assert.NotEmpty(config.LatencyBuckets);
        Assert.NotEmpty(config.DefaultLabels);
    }

    [Fact]
    public void Configuration_ShouldHandleCustomValues()
    {
        // Arrange
        var customConfig = new CustomMetricsConfiguration
        {
            MetricsEndpoint = "/custom-metrics",
            ApplicationName = "custom-app",
            ApplicationVersion = "2.0",
            CartValueBuckets = new[] { 1.0, 2.0, 5.0 },
            LatencyBuckets = new[] { 100.0, 200.0, 500.0 }
        };

        // Act
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IMetricsConfiguration>(customConfig);
        serviceCollection.AddCustomMetrics();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert
        var collector = serviceProvider.GetRequiredService<IMetricCollector>();
        Assert.NotNull(collector);
    }
}

public class CustomMetricsConfiguration : IMetricsConfiguration
{
    public string MetricsEndpoint { get; set; } = "/metrics";
    public string[] ExcludedPaths { get; set; } = new[] { "/metrics", "/health" };
    public double[] LatencyBuckets { get; set; } = Array.Empty<double>();
    public double[] CartValueBuckets { get; set; } = Array.Empty<double>();
    public string ApplicationName { get; set; } = "test-app";
    public string ApplicationVersion { get; set; } = "1.0";
    public IDictionary<string, string> DefaultLabels => new Dictionary<string, string>
    {
        { "app", ApplicationName },
        { "version", ApplicationVersion }
    };
}
