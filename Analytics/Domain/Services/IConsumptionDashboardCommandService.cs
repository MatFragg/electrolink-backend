namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IConsumptionDashboardCommandService
{
    Task InitializeDashboardAsync(string homeownerId, string propertyId, List<string> deviceIds, string planTier);
    Task UpdateConsumptionDashboardAsync(string homeownerId, string deviceId, string circuitId, decimal kWh, decimal voltage, decimal current, DateTime readingTimestamp);
    Task UpgradeDashboardTierAsync(string homeownerId, string newPlanTier);
    Task GenerateCostProjectionAsync(string homeownerId, decimal electricityRatePerKWh);
    Task UpdateConsumptionThresholdsAsync(string homeownerId, Dictionary<string, decimal> thresholds);
}
