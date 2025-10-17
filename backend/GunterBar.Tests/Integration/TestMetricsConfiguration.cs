using System.Collections.Generic;
using GunterBar.Presentation.Metrics;

namespace GunterBar.Tests.Integration;

internal class TestMetricsConfiguration : IMetricsConfiguration
{
    public string MetricsEndpoint => "/metrics";
    public string[] ExcludedPaths => new[] { "/metrics", "/health" };
    public double[] LatencyBuckets => new[] { 10.0, 25.0, 50.0, 100.0 };
    public double[] CartValueBuckets => new[] { 5.0, 10.0, 25.0, 50.0 };
    public string ApplicationName => "test-app";
    public string ApplicationVersion => "1.0";
    public IDictionary<string, string> DefaultLabels => new Dictionary<string, string>
    {
        { "app", ApplicationName },
        { "version", ApplicationVersion },
        { "environment", "test" }
    };
}
