using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public class ServiceCancellationRequest : BaseAggregateRoot
{
    public CancellationRequestId Id { get; private set; } = null!;
    public ServiceExecutionId ExecutionId { get; private set; } = null!;
    public ECancelledBy CancelledBy { get; private set; }
    public UserId ActorId { get; private set; } = null!;
    public required string Reason { get; init; }
    public string? Notes { get; private set; }
    public bool RequestReassignment { get; private set; }
    public DateTime RequestedAt { get; private set; }

    private ServiceCancellationRequest() { }

    public static ServiceCancellationRequest Create(
        ServiceExecutionId executionId,
        ECancelledBy cancelledBy,
        UserId actorId,
        string reason,
        string? notes,
        bool requestReassignment)
    {
        bool effectiveReassignment = cancelledBy != ECancelledBy.Technician && requestReassignment;
        
        return new ServiceCancellationRequest
        {
            Id = CancellationRequestId.NewId(),
            ExecutionId = executionId,
            CancelledBy = cancelledBy,
            ActorId = actorId,
            Reason = reason,
            Notes = notes,
            RequestReassignment = effectiveReassignment,
            RequestedAt = DateTime.UtcNow
        };
    }
}