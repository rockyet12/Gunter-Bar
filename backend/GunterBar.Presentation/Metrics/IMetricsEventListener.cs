namespace GunterBar.Presentation.Metrics;

public interface IMetricsEventListener
{
    void OnMetricRecorded(string metricName, object value, Dictionary<string, string>? labels = null);
}
