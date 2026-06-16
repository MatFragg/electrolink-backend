namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IConsumptionReportCommandService
{
    Task<string> RequestConsumptionReportAsync(string homeownerId, string propertyId, DateTime periodStart, DateTime periodEnd, string planTier, string exportFormat);
}
