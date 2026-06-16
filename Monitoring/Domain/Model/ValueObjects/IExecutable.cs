using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

public interface IExecutable
{
    void Start(TechnicianId technicianId, DateTime startedAt);
    void MarkEnRoute(TechnicianId technicianId, DateTime timestamp);
    void MarkArrived(TechnicianId technicianId, DateTime timestamp);
    void Complete(TechnicianId technicianId, DateTime completedAt);
    void Cancel(CancellationRequestId? cancellationRequestId, string cancelledById, ECancelledBy cancelledBy, ECancellationReason reason, string? notes, bool requestReassignment);
    void TryFinalizeAfterReviews();
}
