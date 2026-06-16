namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record CircuitSummaryEntry(string CircuitId, decimal TotalKilowattHours, decimal PeakVoltage, decimal PeakCurrent, DateTime LastReadingAt);
