using System.Collections.Generic;
using GunterBar.Presentation.Infrastructure;
namespace GunterBar.Tests.Integration;

internal class TestMetricsConfiguration : IMetricsConfiguration
{
    public string MetricsEndpoint => "/metrics";
    public string[] ExcludedPaths => new[] { "/metrics", "/health" };
    public double[] LatencyBuckets => new[] { 10.0, 25.0, 50.0, 100.0 };
    public double[] CartValueBuckets => new[] { 0.1, 0.5, 1.0, 5.0, 10.0, 25.0, 50.0, 100.0, 500.0, 1000.0 };
    public string ApplicationName => "test_app";
    public string ApplicationVersion => "1.0";
    public IDictionary<string, string> DefaultLabels => new Dictionary<string, string>
    {
        { "app", ApplicationName },
        { "version", ApplicationVersion },
        { "environment", "test" }
    };
}
