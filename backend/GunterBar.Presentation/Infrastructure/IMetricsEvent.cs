using System;

namespace GunterBar.Presentation.Infrastructure;

public interface IMetricsEvent
{
    DateTime Timestamp { get; }
    string MetricName { get; }
    string[] Labels { get; }
    double Value { get; }
}

public interface IMetricsEventListener
{
    void OnMetricRecorded(IMetricsEvent metricsEvent);
}
