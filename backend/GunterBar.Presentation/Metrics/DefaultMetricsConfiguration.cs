using System.Collections.Generic;

namespace GunterBar.Presentation.Metrics;

public class DefaultMetricsConfiguration : IMetricsConfiguration
{
    public string MetricsEndpoint => "/metrics";
    
    public string[] ExcludedPaths => new[] 
    { 
        "/metrics",
        "/health",
        "/favicon.ico"
    };
    
    public double[] LatencyBuckets => new[] 
    { 
        10.0, 25.0, 50.0, 100.0, 250.0, 
        500.0, 1000.0, 2500.0, 5000.0 
    };
    
    public double[] CartValueBuckets => new[] 
    { 
        5.0, 10.0, 25.0, 50.0, 
        100.0, 250.0, 500.0 
    };
    
    public string ApplicationName => "gunterbar";
    
    public string ApplicationVersion => "1.0";
    
    public IDictionary<string, string> DefaultLabels => new Dictionary<string, string>
    {
        ["app"] = ApplicationName,
        ["version"] = ApplicationVersion
    };
}
