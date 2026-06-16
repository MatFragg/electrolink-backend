using Hampcoders.Electrolink.API.Analytics.Application.Internal.Services;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Entities;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Services;

public class DashboardProjectionService(AppDbContext context) : IDashboardProjectionService
{
    public async Task RecordTimeSeriesAsync(string dashboardId, DateTime normalizedTimestamp, decimal kWh, string granularity)
    {
        var existing = await context.Set<TimeSeriesRecord>()
            .FirstOrDefaultAsync(r =>
                r.DashboardId == dashboardId &&
                r.Timestamp == normalizedTimestamp &&
                r.Granularity == granularity);

        if (existing != null)
        {
            existing.KilowattHours += kWh;
        }
        else
        {
            context.Set<TimeSeriesRecord>().Add(new TimeSeriesRecord
            {
                DashboardId = dashboardId,
                Timestamp = normalizedTimestamp,
                KilowattHours = kWh,
                Granularity = granularity
            });
        }
    }

    public async Task UpsertCircuitSummaryAsync(string dashboardId, string circuitId, decimal kWh, decimal voltage, decimal current, DateTime readingAt)
    {
        var existing = await context.Set<CircuitSummaryRecord>()
            .FirstOrDefaultAsync(r =>
                r.DashboardId == dashboardId &&
                r.CircuitId == circuitId);

        if (existing != null)
        {
            existing.TotalKilowattHours += kWh;
            existing.PeakVoltage = Math.Max(existing.PeakVoltage, voltage);
            existing.PeakCurrent = Math.Max(existing.PeakCurrent, current);
            existing.LastReadingAt = readingAt;
        }
        else
        {
            context.Set<CircuitSummaryRecord>().Add(new CircuitSummaryRecord
            {
                DashboardId = dashboardId,
                CircuitId = circuitId,
                TotalKilowattHours = kWh,
                PeakVoltage = voltage,
                PeakCurrent = current,
                LastReadingAt = readingAt
            });
        }
    }

    public async Task<List<TimeSeriesEntry>> GetTimeSeriesAsync(string dashboardId)
    {
        return await context.Set<TimeSeriesRecord>()
            .Where(r => r.DashboardId == dashboardId)
            .OrderBy(r => r.Timestamp)
            .Select(r => new TimeSeriesEntry(r.Timestamp, r.KilowattHours, r.Granularity))
            .ToListAsync();
    }

    public async Task<List<CircuitSummaryEntry>> GetCircuitSummariesAsync(string dashboardId)
    {
        return await context.Set<CircuitSummaryRecord>()
            .Where(r => r.DashboardId == dashboardId)
            .Select(r => new CircuitSummaryEntry(r.CircuitId, r.TotalKilowattHours, r.PeakVoltage, r.PeakCurrent, r.LastReadingAt))
            .ToListAsync();
    }

    public async Task<decimal> GetAccumulatedKWhForMonthAsync(string dashboardId, int year, int month)
    {
        var monthStart = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var monthEnd = monthStart.AddMonths(1);

        return await context.Set<TimeSeriesRecord>()
            .Where(r => r.DashboardId == dashboardId && r.Timestamp >= monthStart && r.Timestamp < monthEnd)
            .SumAsync(r => r.KilowattHours);
    }
}
