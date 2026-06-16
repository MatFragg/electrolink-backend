using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

public class RelayActionRecord
{
    public RelayActionId Id { get; private set; } = null!;
    public ServiceExecutionId ExecutionId { get; private set; } = null!;
    public DeviceId DeviceId { get; private set; } = null!;
    public ERelayActionStatus Status { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public DateTime? ExecutedAt { get; private set; }
    public string? FailureReason { get; private set; }

    protected RelayActionRecord() { }

    private RelayActionRecord(
        RelayActionId id,
        ServiceExecutionId executionId,
        DeviceId deviceId,
        DateTime issuedAt)
    {
        Id = id;
        ExecutionId = executionId;
        DeviceId = deviceId;
        Status = ERelayActionStatus.Pending;
        IssuedAt = issuedAt;
    }

    public static RelayActionRecord Create(
        ServiceExecutionId executionId,
        DeviceId deviceId,
        DateTime issuedAt)
    {
        return new RelayActionRecord(
            RelayActionId.NewRelayActionId(),
            executionId,
            deviceId,
            issuedAt);
    }

    public void MarkExecuted(DateTime executedAt)
    {
        Status = ERelayActionStatus.Executed;
        ExecutedAt = executedAt;
    }

    public void MarkFailed(string failureReason, DateTime failedAt)
    {
        Status = ERelayActionStatus.Failed;
        FailureReason = failureReason;
        ExecutedAt = failedAt;
    }
}
