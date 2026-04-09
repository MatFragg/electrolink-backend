using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;

/// <summary>
/// Event published when a work photo is uploaded.
/// </summary>
public record WorkPhotoUploadedEvent(
    ServiceExecutionId ExecutionId,
    WorkPhotoId PhotoId,
    EPhotoType PhotoType,
    DateTime OccurredOn
) : IEvent
{
public Guid EventId { get; init; } = Guid.NewGuid();
}

