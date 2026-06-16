using Hampcoders.Electrolink.API.Analytics.Application.Internal.QueryServices;

using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.Services;

public interface IDashboardProjectionService
{
    Task RecordTimeSeriesAsync(string dashboardId, DateTime normalizedTimestamp, decimal kWh, string granularity);
    Task UpsertCircuitSummaryAsync(string dashboardId, string circuitId, decimal kWh, decimal voltage, decimal current, DateTime readingAt);
    Task<List<TimeSeriesEntry>> GetTimeSeriesAsync(string dashboardId);
    Task<List<CircuitSummaryEntry>> GetCircuitSummariesAsync(string dashboardId);
    Task<decimal> GetAccumulatedKWhForMonthAsync(string dashboardId, int year, int month);
}
