namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Entities;

public class TimeSeriesRecord
{
    public Guid Id { get; set; }
    public string DashboardId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public decimal KilowattHours { get; set; }
    public string Granularity { get; set; } = string.Empty;
}
