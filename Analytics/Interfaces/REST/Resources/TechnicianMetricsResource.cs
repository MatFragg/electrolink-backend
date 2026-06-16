namespace Hampcoders.Electrolink.API.Analytics.Interfaces.REST.Resources;

public record TechnicianMetricsResource(
    string MetricsId,
    string TechnicianId,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    int CompletedServicesCount,
    int IoTServicesCount,
    decimal AverageRating,
    decimal TotalRevenueAmount,
    string TotalRevenueCurrency,
    TimeSpan AverageResponseTime,
    decimal ClientRetentionRate,
    DateTime LastCalculatedAt);
