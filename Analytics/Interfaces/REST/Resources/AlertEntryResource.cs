namespace Hampcoders.Electrolink.API.Analytics.Interfaces.REST.Resources;

public record AlertEntryResource(
    string EntryId,
    string SourceEventId,
    string SourceBC,
    string AlertType,
    string Severity,
    string Status,
    string? CircuitId,
    string? ServiceRequestId,
    DateTime TriggeredAt,
    DateTime? AcknowledgedAt,
    DateTime? ResolvedAt);
