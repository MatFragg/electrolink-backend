namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record CircuitSummary
{
    public string CircuitId { get; }
    public decimal TotalKilowattHours { get; }
    public decimal PeakVoltage { get; }
    public decimal PeakCurrent { get; }
    public DateTime LastReadingAt { get; }

    private CircuitSummary(string circuitId, decimal totalKilowattHours, decimal peakVoltage,
        decimal peakCurrent, DateTime lastReadingAt)
    {
        if (string.IsNullOrWhiteSpace(circuitId))
            throw new ArgumentException("CircuitId cannot be empty.");
        CircuitId = circuitId;
        TotalKilowattHours = totalKilowattHours;
        PeakVoltage = peakVoltage;
        PeakCurrent = peakCurrent;
        LastReadingAt = lastReadingAt;
    }

    public static CircuitSummary Of(string circuitId, decimal totalKilowattHours, decimal peakVoltage,
        decimal peakCurrent, DateTime lastReadingAt) =>
        new(circuitId, totalKilowattHours, peakVoltage, peakCurrent, lastReadingAt);
}
