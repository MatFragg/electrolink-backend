namespace Hampcoders.Electrolink.API.Analytics.Interfaces.REST.Resources;

public record CircuitSummaryResource(
    string CircuitId,
    decimal TotalKilowattHours,
    decimal PeakVoltage,
    decimal PeakCurrent,
    DateTime LastReadingAt);
