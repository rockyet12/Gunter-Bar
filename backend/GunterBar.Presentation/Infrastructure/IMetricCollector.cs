using Prometheus;

namespace GunterBar.Presentation.Infrastructure;

public interface IMetricCollector
{
    void OrderCreated(string status);
    void DrinkAdded(string category);
    void UpdateActiveUsers(int count);
    Prometheus.ITimer BeginOrderProcessing();
    void RecordCartValue(decimal value);
    void RecordApiLatency(string endpoint, double milliseconds);
    void RecordLoginAttempt(bool success);
    void IncrementErrorCount(string type);
}
