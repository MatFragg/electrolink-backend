using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record DashboardView(
    ConsumptionDashboard Dashboard,
    List<TimeSeriesEntry> TimeSeries,
    List<CircuitSummaryEntry> CircuitSummaries);
