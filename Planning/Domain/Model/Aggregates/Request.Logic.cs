using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public partial class Request
{
    /// <summary>
    /// Assigns a technician to this request
    /// Can only be called when status is Pending
    /// </summary>
    public void AssignTechnician(TechnicianId technicianId)
    {
        if (Status != RequestStatus.Pending)
        {
            throw new InvalidStateTransitionException(
                Status.Value,
                "Cannot assign technician to non-pending request"
            );
        }

        if (technicianId == null)
        {
            throw new DomainException("TechnicianId cannot be null");
        }

        TechnicianId = technicianId;
        ChangeStatus(RequestStatus.Confirmed);

        AddDomainEvent(new TechnicianAssignedEvent(
            Id,
            technicianId,
            ClientId,
            ServiceId,
            ScheduledDate,
            DateTime.UtcNow
        ));
    }

    /// <summary>
    /// Updates the scheduled date for the request
    /// Cannot be changed if request is completed or cancelled
    /// </summary>
    public void UpdateScheduledDate(DateOnly newDate)
    {
        if (Status == RequestStatus.Completed || Status == RequestStatus.Cancelled)
        {
            throw new DomainException("Cannot update scheduled date for completed or cancelled requests");
        }

        if (newDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainException("Scheduled date cannot be in the past");
        }

        var previousDate = ScheduledDate;
        ScheduledDate = newDate;

        AddDomainEvent(new RequestScheduledDateUpdatedEvent(
            Id,
            previousDate,
            newDate,
            DateTime.UtcNow
        ));
    }

    /// <summary>
    /// Updates the problem description
    /// </summary>
    public void UpdateProblemDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
        {
            throw new DomainException("Problem description cannot be empty");
        }

        ProblemDescription = newDescription;
    }

    /// <summary>
    /// Cancels the request
    /// </summary>
    public void Cancel(string reason)
    {
        if (Status == RequestStatus.Completed)
        {
            throw new DomainException("Cannot cancel a completed request");
        }

        if (Status == RequestStatus.Cancelled)
        {
            return; // Already cancelled, idempotent operation
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new DomainException("Cancellation reason is required");
        }

        ChangeStatus(RequestStatus.Cancelled);

        AddDomainEvent(new RequestCancelledEvent(
            Id,
            ClientId,
            TechnicianId,
            reason,
            DateTime.UtcNow
        ));
    }

    /// <summary>
    /// Changes the status of the request with validation
    /// </summary>
    public void ChangeStatus(RequestStatus newStatus)
    {
        if (!Status.CanTransitionTo(newStatus))
        {
            throw new InvalidStateTransitionException(Status.Value, newStatus.Value);
        }

        var previousStatus = Status;
        Status = newStatus;

        AddDomainEvent(new RequestStatusChangedEvent(
            Id,
            previousStatus,
            newStatus,
            DateTime.UtcNow
        ));
    }

    /// <summary>
    /// Adds a photo to the request
    /// </summary>
    public void AddPhoto(string photoId, string url)
    {
        if (string.IsNullOrWhiteSpace(photoId))
        {
            throw new DomainException("PhotoId cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new DomainException("Photo URL cannot be empty");
        }

        if (Photos.Any(p => p.PhotoId == photoId))
        {
            return; // Photo already exists, idempotent operation
        }

        Photos.Add(new RequestPhoto(photoId, url));
    }


    /// <summary>
    /// Marks the request as in progress
    /// Can only be called by assigned technician when status is Confirmed
    /// </summary>
    public void StartService()
    {
        if (Status != RequestStatus.Confirmed)
        {
            throw new InvalidStateTransitionException(
                Status.Value,
                "Request must be confirmed before starting service"
            );
        }

        if (TechnicianId == null)
        {
            throw new DomainException("Cannot start service without assigned technician");
        }

        ChangeStatus(RequestStatus.InProgress);
    }

    /// <summary>
    /// Marks the request as completed
    /// Can only be called when status is InProgress
    /// </summary>
    public void CompleteService()
    {
        if (Status != RequestStatus.InProgress)
        {
            throw new InvalidStateTransitionException(
                Status.Value,
                "Request must be in progress before completing"
            );
        }

        ChangeStatus(RequestStatus.Completed);
    }
}