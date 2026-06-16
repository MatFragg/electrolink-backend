using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IAlertLogQueryService
{
    Task<AlertLog?> GetAlertHistoryAsync(string homeownerId, string? filterType = null, string? filterSeverity = null, string? filterStatus = null, DateTime? fromDate = null);
    Task<AlertLog?> GetAnomalyImpactSummaryAsync(string homeownerId, DateTime fromDate, DateTime toDate);
}
