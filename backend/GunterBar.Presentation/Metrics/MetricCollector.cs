namespace GunterBar.Presentation.Metrics;

public class MetricCollector : IMetricCollector
{
    private readonly IMetricsEventListener _eventListener;

    public MetricCollector(IMetricsEventListener eventListener)
    {
        _eventListener = eventListener;
    }

    public void IncrementErrorCount(string errorType)
    {
        _eventListener.OnMetricRecorded("errors_total", 1, new() { { "error_type", errorType } });
    }

    public void RecordApiLatency(string endpoint, long milliseconds)
    {
        _eventListener.OnMetricRecorded("api_latency_milliseconds", milliseconds, new() { { "endpoint", endpoint } });
    }

    public void UpdateActiveUsers(int count)
    {
        _eventListener.OnMetricRecorded("active_users", count);
    }

    public void RecordLoginAttempt(bool success)
    {
        _eventListener.OnMetricRecorded("login_attempts_total", 1, new() { { "success", success.ToString().ToLower() } });
    }
}
