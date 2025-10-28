namespace GunterBar.Presentation.Metrics;

public interface IMetricCollector
{
    void IncrementErrorCount(string errorType);
    void RecordApiLatency(string endpoint, long milliseconds);
    void UpdateActiveUsers(int count);
    void RecordLoginAttempt(bool success);
}
