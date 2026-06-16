using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Enums;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using ServiceRequestId = Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects.ServiceRequestId;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

public class AlertLog : BaseAggregateRoot
{
    public AlertLogId LogId { get; private set; }
    public HomeownerId HomeownerId { get; private set; }
    public List<AlertEntry> Entries { get; private set; }

    private AlertLog() { }

    public static AlertLog CreateFor(HomeownerId homeownerId)
    {
        var log = new AlertLog
        {
            LogId = AlertLogId.New(),
            HomeownerId = homeownerId ?? throw new ArgumentNullException(nameof(homeownerId)),
            Entries = []
        };
        return log;
    }

    public AlertEntry RecordAlert(
        SourceEventId sourceEventId,
        AlertSourceBC sourceBC,
        AlertType alertType,
        AlertSeverity severity,
        string? circuitId = null)
    {
        if (Entries.Any(e => e.SourceEventId.Value == sourceEventId.Value))
            throw new InvalidOperationException(
                $"Alert with sourceEventId '{sourceEventId.Value}' already registered.");

        var entry = AlertEntry.Create(sourceEventId, sourceBC, alertType, severity, circuitId);
        Entries.Add(entry);

        RaiseDomainEvent(new AlertRecorded(
            LogId.Value,
            entry.EntryId.Value,
            HomeownerId.Value,
            alertType.ToString(),
            severity.ToString(),
            circuitId,
            DateTime.UtcNow));

        return entry;
    }

    public void AcknowledgeAlert(AlertEntryId entryId)
    {
        var entry = FindActiveEntry(entryId);
        entry.Acknowledge();

        RaiseDomainEvent(new AlertAcknowledged(
            LogId.Value, entry.EntryId.Value, HomeownerId.Value, DateTime.UtcNow));
    }

    public void ResolveAlertBySourceEvent(SourceEventId sourceEventId)
    {
        var entry = Entries.FirstOrDefault(e =>
            e.SourceEventId.Value == sourceEventId.Value &&
            e.Status != AlertStatus.Resolved);

        if (entry == null)
            throw new InvalidOperationException(
                $"No active alert found for sourceEventId '{sourceEventId.Value}'.");

        entry.Resolve();

        RaiseDomainEvent(new AlertResolved(
            LogId.Value, entry.EntryId.Value, HomeownerId.Value,
            sourceEventId.Value, DateTime.UtcNow));
    }

    public void LinkAlertToServiceRequest(AlertEntryId entryId, ServiceRequestId serviceRequestId)
    {
        var entry = Entries.FirstOrDefault(e => e.EntryId.Value == entryId.Value)
            ?? throw new InvalidOperationException($"AlertEntry '{entryId.Value}' not found.");
        entry.LinkToServiceRequest(serviceRequestId);

        RaiseDomainEvent(new AlertLinkedToServiceRequest(
            LogId.Value, entry.EntryId.Value,
            serviceRequestId.Value, DateTime.UtcNow));
    }

    private AlertEntry FindActiveEntry(AlertEntryId entryId) =>
        Entries.FirstOrDefault(e =>
            e.EntryId.Value == entryId.Value && e.Status == AlertStatus.Active)
        ?? throw new InvalidOperationException(
            $"No active alert found with id '{entryId.Value}'.");
}
