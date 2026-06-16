using Hampcoders.Electrolink.API.Analytics.Domain.Model.Enums;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;

public class AlertEntry
{
    public AlertEntryId EntryId { get; private set; }
    public SourceEventId SourceEventId { get; private set; }
    public AlertSourceBC SourceBC { get; private set; }
    public AlertType AlertType { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public string? CircuitId { get; private set; }
    public DateTime TriggeredAt { get; private set; }
    public DateTime? AcknowledgedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public AlertStatus Status { get; private set; }
    public ServiceRequestId? LinkedServiceRequestId { get; private set; }

    private AlertEntry() { }

    public static AlertEntry Create(
        SourceEventId sourceEventId,
        AlertSourceBC sourceBC,
        AlertType alertType,
        AlertSeverity severity,
        string? circuitId = null)
    {
        return new AlertEntry
        {
            EntryId = AlertEntryId.New(),
            SourceEventId = sourceEventId,
            SourceBC = sourceBC,
            AlertType = alertType,
            Severity = severity,
            CircuitId = circuitId,
            TriggeredAt = DateTime.UtcNow,
            Status = AlertStatus.Active
        };
    }

    internal void Acknowledge()
    {
        if (Status != AlertStatus.Active)
            throw new InvalidOperationException("Only active alerts can be acknowledged.");
        Status = AlertStatus.Acknowledged;
        AcknowledgedAt = DateTime.UtcNow;
    }

    internal void Resolve()
    {
        Status = AlertStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
    }

    internal void LinkToServiceRequest(ServiceRequestId serviceRequestId)
    {
        LinkedServiceRequestId = serviceRequestId;
    }
}
