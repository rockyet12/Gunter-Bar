namespace GunterBar.Presentation.Metrics;

public interface IMetricsConfiguration
{
    string MetricsEndpoint { get; }
    string[] ExcludedPaths { get; }
    double[] LatencyBuckets { get; }
    double[] CartValueBuckets { get; }
    string ApplicationName { get; }
    string ApplicationVersion { get; }
    
    IDictionary<string, string> DefaultLabels { get; }
}
