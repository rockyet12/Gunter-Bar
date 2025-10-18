using System;

namespace GunterBar.Presentation.Infrastructure;

public class MetricsEvent : IMetricsEvent
{
    public DateTime Timestamp { get; }
    public string MetricName { get; }
    public string[] Labels { get; }
    public double Value { get; }

    public MetricsEvent(string metricName, double value, string[]? labels = default)
    {
        Timestamp = DateTime.UtcNow;
        MetricName = metricName;
        Value = value;
        Labels = labels ?? Array.Empty<string>();
    }
}

public class ConsoleMetricsListener : IMetricsEventListener
{
    public void OnMetricRecorded(IMetricsEvent metricsEvent)
    {
        Console.WriteLine($"[{metricsEvent.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
                        $"Metric: {metricsEvent.MetricName}, " +
                        $"Value: {metricsEvent.Value}, " +
                        $"Labels: [{string.Join(", ", metricsEvent.Labels)}]");
    }
}
