using Hampcoders.Electrolink.API.Processing.Domain.Model.Events;
using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;

public class RelayControlCommand : BaseAggregateRoot
{
    public RelayCommandId  CommandId          { get; private set; }
    public DeviceId        DeviceId           { get; private set; }
    public PropertyId      PropertyId         { get; private set; }

    public ERelayTarget         TargetRelayState    { get; private set; }
    public string               RequestedBy         { get; private set; }
    public string               AuthorizationSource { get; private set; }
    public ServiceRequestId?    ServiceRequestId    { get; private set; }

    public ERelayCommandStatus Status          { get; private set; }
    public DateTime            IssuedAt        { get; private set; }
    public DateTime?           SentAt          { get; private set; }
    public DateTime?           AcknowledgedAt  { get; private set; }
    public DateTime?           ExecutedAt      { get; private set; }
    public string?             FailureReason   { get; private set; }
    public int                 RetryCount      { get; private set; }

    private RelayControlCommand() { }

    public static RelayControlCommand IssueByTechnician(
        DeviceId deviceId,
        PropertyId propertyId,
        ERelayTarget targetState,
        TechnicianId technicianId,
        ServiceRequestId serviceRequestId)
    {
        var cmd = new RelayControlCommand
        {
            CommandId           = RelayCommandId.NewId(),
            DeviceId            = deviceId,
            PropertyId          = propertyId,
            TargetRelayState    = targetState,
            RequestedBy         = technicianId.Value,
            AuthorizationSource = "SERVICE_REQUEST",
            ServiceRequestId    = serviceRequestId,
            Status              = ERelayCommandStatus.Pending,
            IssuedAt            = DateTime.UtcNow,
            RetryCount          = 0,
        };

        cmd.RaiseDomainEvent(new RelayCommandIssuedEvent(
            cmd.CommandId.Value, deviceId.Value, propertyId.Value,
            targetState.ToString(), technicianId.Value, "SERVICE_REQUEST",
            serviceRequestId.Value, cmd.IssuedAt));

        return cmd;
    }

    public static RelayControlCommand IssueBySystem(
        DeviceId deviceId,
        PropertyId propertyId,
        ERelayTarget targetState)
    {
        var cmd = new RelayControlCommand
        {
            CommandId           = RelayCommandId.NewId(),
            DeviceId            = deviceId,
            PropertyId          = propertyId,
            TargetRelayState    = targetState,
            RequestedBy         = "SYSTEM",
            AuthorizationSource = "CRITICAL_ANOMALY_AUTO",
            Status              = ERelayCommandStatus.Pending,
            IssuedAt            = DateTime.UtcNow,
            RetryCount          = 0,
        };

        cmd.RaiseDomainEvent(new RelayCommandIssuedEvent(
            cmd.CommandId.Value, deviceId.Value, propertyId.Value,
            targetState.ToString(), "SYSTEM", "CRITICAL_ANOMALY_AUTO",
            null, cmd.IssuedAt));

        return cmd;
    }

    public void MarkAsSent()
    {
        if (Status != ERelayCommandStatus.Pending)
            throw new InvalidOperationException($"Cannot mark as SENT from status {Status}.");

        Status = ERelayCommandStatus.Sent;
        SentAt = DateTime.UtcNow;
    }

    public void MarkAsAcknowledged()
    {
        if (Status != ERelayCommandStatus.Sent)
            throw new InvalidOperationException($"Cannot acknowledge from status {Status}.");

        Status         = ERelayCommandStatus.Acknowledged;
        AcknowledgedAt = DateTime.UtcNow;
    }

    public void MarkAsExecuted()
    {
        if (Status is not (ERelayCommandStatus.Sent or ERelayCommandStatus.Acknowledged))
            throw new InvalidOperationException($"Cannot mark as EXECUTED from status {Status}.");

        Status     = ERelayCommandStatus.Executed;
        ExecutedAt = DateTime.UtcNow;

        RaiseDomainEvent(new RelayCommandExecutedEvent(
            CommandId.Value, DeviceId.Value, PropertyId.Value,
            TargetRelayState.ToString(), RequestedBy,
            ServiceRequestId?.Value, ExecutedAt.Value));
    }

    public void MarkAsTimeout()
    {
        Status        = ERelayCommandStatus.Timeout;
        FailureReason = "No acknowledgment received within 30 seconds.";

        RaiseDomainEvent(new RelayCommandFailedEvent(
            CommandId.Value, DeviceId.Value, PropertyId.Value,
            TargetRelayState.ToString(), FailureReason, DateTime.UtcNow));
    }

    public void MarkAsFailed(string reason)
    {
        Status        = ERelayCommandStatus.Failed;
        FailureReason = reason;
        RetryCount++;

        RaiseDomainEvent(new RelayCommandFailedEvent(
            CommandId.Value, DeviceId.Value, PropertyId.Value,
            TargetRelayState.ToString(), reason, DateTime.UtcNow));
    }
}
