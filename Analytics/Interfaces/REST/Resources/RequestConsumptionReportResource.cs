namespace Hampcoders.Electrolink.API.Analytics.Interfaces.REST.Resources;

public record RequestConsumptionReportResource(
    string HomeownerId,
    string PropertyId,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    string ExportFormat);
