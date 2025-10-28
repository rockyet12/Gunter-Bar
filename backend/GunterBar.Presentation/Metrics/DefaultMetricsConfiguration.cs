namespace GunterBar.Presentation.Metrics;

public class DefaultMetricsConfiguration : IMetricsConfiguration
{
    public string MetricsEndpoint => "/metrics";
    public string[] ExcludedPaths => new[] { "/metrics", "/health" };
    public double[] LatencyBuckets => new[] { 0.1, 0.5, 1.0, 2.5, 5.0, 10.0 };
    public double[] CartValueBuckets => new[] { 10.0, 25.0, 50.0, 100.0, 250.0 };
    public string ApplicationName => "GunterBar";
    public string ApplicationVersion => "1.0.0";
    public IDictionary<string, string> DefaultLabels => new Dictionary<string, string>
    {
        { "app", ApplicationName },
        { "version", ApplicationVersion }
    };
}
