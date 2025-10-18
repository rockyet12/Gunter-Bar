using System.Collections.Generic;

namespace GunterBar.Presentation.Infrastructure;

public interface IMetricsConfiguration
{
    string MetricsEndpoint { get; }
    IDictionary<string, string> DefaultLabels { get; }
    string[] ExcludedPaths { get; }
    double[] LatencyBuckets { get; }
    double[] CartValueBuckets { get; }
    string ApplicationName { get; }
    string ApplicationVersion { get; }
}
