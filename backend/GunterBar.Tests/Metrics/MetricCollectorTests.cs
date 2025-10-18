using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using GunterBar.Presentation.Infrastructure;

namespace GunterBar.Tests.Metrics;

public class MetricCollectorTests
{
    private readonly Mock<IMetricsConfiguration> _configMock;
    private readonly Mock<IMetricsEventListener> _listenerMock;
    private readonly IMetricCollector _collector;
    private readonly IDictionary<string, string> _defaultLabels;

    public MetricCollectorTests()
    {
        _defaultLabels = new Dictionary<string, string>
        {
            { "app", "test_app" },
            { "version", "1.0.0" }
        };

        _configMock = new Mock<IMetricsConfiguration>();
        _configMock.Setup(c => c.ApplicationName).Returns("test_app");
        _configMock.Setup(c => c.DefaultLabels).Returns(_defaultLabels);
        _configMock.Setup(c => c.CartValueBuckets).Returns(new[] { 5.0, 10.0, 25.0, 50.0, 100.0 });
        _configMock.Setup(c => c.LatencyBuckets).Returns(new[] { 10.0, 25.0, 50.0, 100.0 });

        _listenerMock = new Mock<IMetricsEventListener>();
        _collector = new MetricCollector(_configMock.Object, _listenerMock.Object);
    }

    [Fact]
    public void OrderCreated_ShouldIncrementCounter_AndNotifyListener()
    {
        // Arrange
        const string status = "completed";

        // Act
        _collector.OrderCreated(status);

        // Assert
        _listenerMock.Verify(l => l.OnMetricRecorded(
            It.Is<IMetricsEvent>(e =>
                e.MetricName == "orders_total" &&
                e.Value == 1 &&
                e.Labels.Contains(status)
            )
        ), Times.Once);
    }

    [Fact]
    public void DrinkAdded_ShouldIncrementCounter_AndNotifyListener()
    {
        // Arrange
        const string category = "cocktail";

        // Act
        _collector.DrinkAdded(category);

        // Assert
        _listenerMock.Verify(l => l.OnMetricRecorded(
            It.Is<IMetricsEvent>(e =>
                e.MetricName == "drinks_added_total" &&
                e.Value == 1 &&
                e.Labels.Contains(category)
            )
        ), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    public void UpdateActiveUsers_ShouldUpdateGauge_AndNotifyListener(int delta)
    {
        // Act
        _collector.UpdateActiveUsers(delta);

        // Assert
        _listenerMock.Verify(l => l.OnMetricRecorded(
            It.Is<IMetricsEvent>(e =>
                e.MetricName == "active_users" &&
                e.Value == delta
            )
        ), Times.Once);
    }

    [Fact]
    public void BeginOrderProcessing_ShouldReturnTimer()
    {
        // Act
        var timer = _collector.BeginOrderProcessing();

        // Assert
        Assert.NotNull(timer);
    }

    [Theory]
    [InlineData(10.5)]
    [InlineData(25.0)]
    [InlineData(50.0)]
    public void RecordCartValue_ShouldObserveValue_AndNotifyListener(decimal value)
    {
        // Act
        _collector.RecordCartValue(value);

        // Assert
        _listenerMock.Verify(l => l.OnMetricRecorded(
            It.Is<IMetricsEvent>(e =>
                e.MetricName == "cart_value_dollars" &&
                Math.Abs(e.Value - (double)value) < 0.001
            )
        ), Times.Once);
    }

    [Theory]
    [InlineData("/api/orders", 150.0)]
    [InlineData("/api/drinks", 75.5)]
    public void RecordApiLatency_ShouldObserveValue_AndNotifyListener(string endpoint, double milliseconds)
    {
        // Act
        _collector.RecordApiLatency(endpoint, milliseconds);

        // Assert
        _listenerMock.Verify(l => l.OnMetricRecorded(
            It.Is<IMetricsEvent>(e =>
                e.MetricName == "api_latency_milliseconds" &&
                Math.Abs(e.Value - milliseconds) < 0.001 &&
                e.Labels.Contains(endpoint)
            )
        ), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void RecordLoginAttempt_ShouldIncrementCounter_AndNotifyListener(bool success)
    {
        // Act
        _collector.RecordLoginAttempt(success);

        // Assert
        _listenerMock.Verify(l => l.OnMetricRecorded(
            It.Is<IMetricsEvent>(e =>
                e.MetricName == "login_attempts_total" &&
                e.Value == 1 &&
                e.Labels.Contains(success.ToString().ToLower())
            )
        ), Times.Once);
    }

    [Fact]
    public void IncrementErrorCount_ShouldIncrementCounter_AndNotifyListener()
    {
        // Arrange
        const string errorType = "validation_error";

        // Act
        _collector.IncrementErrorCount(errorType);

        // Assert
        _listenerMock.Verify(l => l.OnMetricRecorded(
            It.Is<IMetricsEvent>(e =>
                e.MetricName == "errors_total" &&
                e.Value == 1 &&
                e.Labels.Contains(errorType)
            )
        ), Times.Once);
    }
}
