using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public record ReadingIngestedIntegrationEvent(
    string HomeownerId,
    string DeviceId,
    string CircuitId,
    decimal KilowattHours,
    decimal Voltage,
    decimal Current,
    DateTime ReadingTimestamp) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ReadingTimestamp;
}

public record AnomalyDetectedIntegrationEvent(
    string HomeownerId,
    string AlertId,
    string DeviceId,
    string AnomalyType,
    string Severity,
    DateTime Timestamp,
    string Description) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = Timestamp;
}

public record AnomalyResolvedIntegrationEvent(
    string HomeownerId,
    string AlertId,
    DateTime ResolutionTimestamp) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ResolutionTimestamp;
}

public record DeviceDisconnectedIntegrationEvent(
    string HomeownerId,
    string DeviceId,
    DateTime DisconnectedAt) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DisconnectedAt;
}

public record DeviceReconnectedIntegrationEvent(
    string HomeownerId,
    string DeviceId,
    DateTime ReconnectedAt) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ReconnectedAt;
}

public record RelayCommandExecutedIntegrationEvent(
    string HomeownerId,
    string DeviceId,
    string CircuitId,
    string CommandType,
    DateTime ExecutedAt,
    bool Success) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ExecutedAt;
}

public record ServiceCompletedIntegrationEvent(
    string HomeownerId,
    string ServiceId,
    string ServiceType,
    DateTime CompletedAt,
    string TechnicianId,
    decimal ServiceRevenue,
    string Currency,
    TimeSpan ResponseTime,
    bool RequiresIoTCertification) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = CompletedAt;
}

public record TechnicianEvaluationSubmittedIntegrationEvent(
    string TechnicianId,
    int Score,
    string Feedback,
    DateTime SubmittedAt) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = SubmittedAt;
}

public record EnterpriseSubscriptionFullyActiveIntegrationEvent(
    string HomeownerId,
    string SubscriptionId,
    string PropertyId,
    List<string> DeviceIds,
    DateTime ActivatedAt) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = ActivatedAt;
}

public record ConsumptionThresholdsUpdatedIntegrationEvent(
    string HomeownerId,
    decimal LowThreshold,
    decimal HighThreshold,
    DateTime UpdatedAt) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = UpdatedAt;
}

public record ServiceSuggestionAcceptedIntegrationEvent(
    string HomeownerId,
    string ServiceSuggestionId,
    DateTime AcceptedAt) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = AcceptedAt;
}
