using System;
using System.Linq;
using System.Collections.Generic;
using Prometheus;


namespace GunterBar.Presentation.Infrastructure;

/// <summary>
/// Implementación del colector de métricas usando Prometheus.
/// </summary>
public class MetricCollector : IMetricCollector
{
    private const string ORDER_METRIC = "orders_total";
    private const string DRINKS_METRIC = "drinks_added_total";
    private const string CART_METRIC = "cart_value_dollars";
    private const string USERS_METRIC = "active_users";
    private const string PROCESSING_METRIC = "order_processing_seconds";
    private const string LATENCY_METRIC = "api_latency_milliseconds";
    private const string LOGIN_METRIC = "login_attempts_total";
    private const string ERROR_METRIC = "errors_total";

    private readonly Counter _ordersCreated;
    private readonly Counter _drinksAdded;
    private readonly Gauge _activeUsers;
    private readonly Histogram _orderProcessingTime;
    private readonly Histogram _cartValue;
    private readonly Histogram _apiLatency;
    private readonly Counter _loginAttempts;
    private readonly Counter _errorCount;
    private readonly IMetricsEventListener? _eventListener;
    private readonly IMetricsConfiguration _configuration;
    private readonly string _prefix;

    private static string[] GetLabelNames(IDictionary<string, string> staticLabels, params string[] additionalLabels)
    {
        return staticLabels.Keys.Concat(additionalLabels).ToArray();
    }

    private string[] GetLabelValues(IDictionary<string, string> staticLabels, params string[] additionalValues)
    {
        return staticLabels.Values.Concat(additionalValues).ToArray();
    }

    /// <summary>
    /// Inicializa una nueva instancia del colector de métricas.
    /// </summary>
    /// <param name="configuration">Configuración de métricas requerida</param>
    /// <param name="eventListener">Listener opcional para eventos de métricas</param>
    /// <exception cref="ArgumentNullException">Si la configuración es nula</exception>
    public MetricCollector(IMetricsConfiguration configuration, IMetricsEventListener? eventListener = null)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _eventListener = eventListener;
        _prefix = $"{_configuration.ApplicationName}_";

        var defaultLabels = _configuration.DefaultLabels;

        // Métricas de negocio
        _ordersCreated = Prometheus.Metrics.CreateCounter(
            $"{_prefix}{ORDER_METRIC}",
            "Total de órdenes creadas",
            new CounterConfiguration 
            { 
                LabelNames = GetLabelNames(defaultLabels, "status")
            }
        );

        _drinksAdded = Prometheus.Metrics.CreateCounter(
            $"{_prefix}{DRINKS_METRIC}",
            "Total de bebidas agregadas al inventario",
            new CounterConfiguration 
            { 
                LabelNames = GetLabelNames(defaultLabels, "category")
            }
        );

        _cartValue = Prometheus.Metrics.CreateHistogram(
            $"{_prefix}{CART_METRIC}",
            "Valor del carrito en dólares",
            new HistogramConfiguration
            {
                Buckets = _configuration.CartValueBuckets,
                LabelNames = GetLabelNames(defaultLabels)
            }
        );

        // Métricas operacionales
        _activeUsers = Prometheus.Metrics.CreateGauge(
            $"{_prefix}{USERS_METRIC}",
            "Número actual de usuarios activos",
            new GaugeConfiguration
            {
                LabelNames = GetLabelNames(defaultLabels)
            }
        );

        _orderProcessingTime = Prometheus.Metrics.CreateHistogram(
            $"{_prefix}{PROCESSING_METRIC}",
            "Tiempo de procesamiento de órdenes",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.1, 2, 10),
                LabelNames = GetLabelNames(defaultLabels)
            }
        );

        _apiLatency = Prometheus.Metrics.CreateHistogram(
            $"{_prefix}{LATENCY_METRIC}",
            "Latencia de endpoints de la API",
            new HistogramConfiguration
            {
                Buckets = _configuration.LatencyBuckets,
                LabelNames = GetLabelNames(defaultLabels, "endpoint")
            }
        );

        // Métricas de seguridad
        _loginAttempts = Prometheus.Metrics.CreateCounter(
            $"{_prefix}{LOGIN_METRIC}",
            "Total de intentos de login",
            new CounterConfiguration 
            { 
                LabelNames = GetLabelNames(defaultLabels, "success")
            }
        );

        _errorCount = Prometheus.Metrics.CreateCounter(
            $"{_prefix}{ERROR_METRIC}",
            "Total de errores por tipo",
            new CounterConfiguration 
            { 
                LabelNames = GetLabelNames(defaultLabels, "type")
            }
        );
    }

    private void NotifyMetricEvent(string metricName, double value, params string[] labels)
    {
        _eventListener?.OnMetricRecorded(new MetricsEvent(metricName, value, labels));
    }

    public void OrderCreated(string status)
    {
        _ordersCreated.WithLabels(GetLabelValues(_configuration.DefaultLabels, status)).Inc();
        NotifyMetricEvent(ORDER_METRIC, 1, status);
    }

    public void DrinkAdded(string category)
    {
        _drinksAdded.WithLabels(GetLabelValues(_configuration.DefaultLabels, category)).Inc();
        NotifyMetricEvent(DRINKS_METRIC, 1, category);
    }

    public void UpdateActiveUsers(int delta)
    {
        _activeUsers.WithLabels(GetLabelValues(_configuration.DefaultLabels)).Inc(delta);
        NotifyMetricEvent(USERS_METRIC, delta);
    }

    Prometheus.ITimer IMetricCollector.BeginOrderProcessing() => 
        _orderProcessingTime.WithLabels(GetLabelValues(_configuration.DefaultLabels)).NewTimer();

    public void RecordCartValue(decimal value)
    {
        var doubleValue = (double)value;
        _cartValue.WithLabels(GetLabelValues(_configuration.DefaultLabels)).Observe(doubleValue);
        NotifyMetricEvent(CART_METRIC, doubleValue);
    }

    public void RecordApiLatency(string endpoint, double milliseconds)
    {
        _apiLatency.WithLabels(GetLabelValues(_configuration.DefaultLabels, endpoint)).Observe(milliseconds);
        NotifyMetricEvent(LATENCY_METRIC, milliseconds, endpoint);
    }

    public void RecordLoginAttempt(bool success)
    {
        var successStr = success.ToString().ToLower();
        _loginAttempts.WithLabels(GetLabelValues(_configuration.DefaultLabels, successStr)).Inc();
        NotifyMetricEvent(LOGIN_METRIC, 1, successStr);
    }

    public void IncrementErrorCount(string type)
    {
        _errorCount.WithLabels(GetLabelValues(_configuration.DefaultLabels, type)).Inc();
        NotifyMetricEvent(ERROR_METRIC, 1, type);
    }
}
