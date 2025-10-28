namespace GunterBar.Presentation.Metrics;

public class ConsoleMetricsListener : IMetricsEventListener
{
    public void OnMetricRecorded(string metricName, object value, Dictionary<string, string>? labels = null)
    {
        var labelStr = labels != null ? string.Join(", ", labels.Select(l => $"{l.Key}={l.Value}")) : "";
        Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Metric: {metricName}, Value: {value}, Labels: [{labelStr}]");
    }
}
