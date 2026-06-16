namespace Hampcoders.Electrolink.API.Analytics.Interfaces.ACL;

public interface IAnalyticsContextFacade
{
    Task<bool> HasActiveAlertsAsync(string homeownerId);
    Task<decimal> GetCurrentPeriodConsumptionAsync(string homeownerId);
    Task<decimal> GetTechnicianAverageRatingAsync(string technicianId);
    Task<bool> HasReportsAvailableAsync(string homeownerId);
}
