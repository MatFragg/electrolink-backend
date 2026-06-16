namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Entities;

public class CircuitSummaryRecord
{
    public Guid Id { get; set; }
    public string DashboardId { get; set; } = string.Empty;
    public string CircuitId { get; set; } = string.Empty;
    public decimal TotalKilowattHours { get; set; }
    public decimal PeakVoltage { get; set; }
    public decimal PeakCurrent { get; set; }
    public DateTime LastReadingAt { get; set; }
}
