using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public class ServiceRequest : BaseAggregateRoot
{
    public RequestId RequestId { get; private set; }
    public HomeownerId HomeownerId { get; private set; }
    public ERequestStatus Status { get; private set; }

    public PropertyId? PropertyId { get; private set; }
    public Geolocation? Geolocation { get; private set; }
    public RecipeId? SelectedRecipeId { get; private set; }
    public EServiceCategory? RequestedCategory { get; private set; }
    public TechnicianId? SelectedTechnicianId { get; private set; }
    public RecipeSnapshot? RecipeSnapshot { get; private set; }
    public AssignmentId? AssignmentId { get; private set; }
    public RequestPreferences? Preferences { get; private set; }
    public bool IsPriority { get; private set; }

    private ServiceRequest() { }

    // ── Factory ───────────────────────────────────────────

    public static ServiceRequest Initiate(
        HomeownerId homeownerId,
        bool canMarkAsPriority,
        int? remainingRequests)
    {
        var request = new ServiceRequest
        {
            RequestId   = RequestId.NewId(),
            HomeownerId = homeownerId,
            Status      = ERequestStatus.Draft,
            IsPriority  = false,
        };
        request.RaiseDomainEvent(new ServiceRequestInitiatedEvent(
            request.RequestId, homeownerId, canMarkAsPriority, remainingRequests, DateTime.UtcNow));
        return request;
    }

    // ── Commands ──────────────────────────────────────────
    public void SelectCategory(EServiceCategory category)
    {
        EnsureStatus(ERequestStatus.PropertySelected);
        RequestedCategory = category;
        Status = ERequestStatus.CategorySelected;
    }
    public void SelectProperty(PropertyId propertyId, Geolocation geolocation)
    {
        EnsureStatus(ERequestStatus.Draft);
        PropertyId  = propertyId;
        Geolocation = geolocation;
        Status = ERequestStatus.PropertySelected;
        RaiseDomainEvent(new PropertySelectedForRequestEvent(RequestId, propertyId, geolocation, DateTime.UtcNow));
    }

    public void SelectRecipe(RecipeId recipeId, RecipeSnapshot snapshot)
    {
        EnsureStatus(ERequestStatus.Draft);
        EnsurePropertySelected();
        SelectedRecipeId = recipeId;
        RecipeSnapshot = snapshot;
    }

    public void AddDetails(RequestPreferences preferences, bool isPriority)
    {
        EnsureStatus(ERequestStatus.CategorySelected);
        //EnsureRecipeSelected();
        Preferences = preferences;
        IsPriority  = isPriority;
        Status = ERequestStatus.ReadyToConfirm;
        RaiseDomainEvent(new ServiceDetailsAddedEvent(RequestId, preferences, isPriority, DateTime.UtcNow));
    }

    public void Confirm()
    {
        EnsureStatus(ERequestStatus.ReadyToConfirm);
        Status = ERequestStatus.PendingAssignment;
        RaiseDomainEvent(new ServiceRequestCreatedEvent(
            RequestId, HomeownerId, PropertyId!, SelectedRecipeId!,
            SelectedTechnicianId!, RecipeSnapshot!, IsPriority, DateTime.UtcNow));
    }

    public void Cancel(CancellationReason reason, string? notes = null)
    {
        var cancellable = new[]
        {
            ERequestStatus.Draft,
            ERequestStatus.ReadyToConfirm,
            ERequestStatus.PendingAssignment
        };

        if (!cancellable.Contains(Status))
            throw new CannotCancelAssignedRequestException(RequestId);

        var wasInQueue = Status == ERequestStatus.PendingAssignment;
        Status = ERequestStatus.Cancelled;
        RaiseDomainEvent(new ServiceRequestCancelledEvent(
            RequestId, HomeownerId, reason, wasInQueue, notes, DateTime.UtcNow));
    }

    public void MarkAsAssigned(AssignmentId assignmentId, TechnicianId technicianId, RecipeSnapshot snapshot)
    {
        EnsureStatus(ERequestStatus.PendingAssignment);
        Status = ERequestStatus.Assigned;
        SelectedTechnicianId = technicianId;
        RecipeSnapshot = snapshot;
        SelectedRecipeId = snapshot.RecipeId;
        AssignmentId = assignmentId;
    }

    public void Expire()
    {
        EnsureStatus(ERequestStatus.PendingAssignment);
        Status = ERequestStatus.Expired;
        RaiseDomainEvent(new ServiceRequestExpiredEvent(RequestId, HomeownerId, DateTime.UtcNow));
    }

    public void Reactivate()
    {
        if (Status != ERequestStatus.Assigned)
            throw new InvalidOperationException(
                $"ServiceRequest {RequestId} must be in Assigned status to reactivate. Current: {Status}.");

        SelectedTechnicianId = null;
        AssignmentId = null;
        RecipeSnapshot = null;
        SelectedRecipeId = null;

        Status = ERequestStatus.PendingAssignment;

        RaiseDomainEvent(new ServiceRequestReactivatedEvent(
            RequestId, HomeownerId, IsPriority, DateTime.UtcNow));
    }
    
    // ── Invariants ────────────────────────────────────────

    private void EnsureStatus(ERequestStatus expected)
    {
        if (Status != expected)
            throw new InvalidRequestStatusException(RequestId, expected, Status);
    }

    private void EnsurePropertySelected()
    {
        if (PropertyId is null)
            throw new PropertyNotSelectedOnRequestException(RequestId);
    }

    private void EnsureRecipeSelected()
    {
        if (SelectedRecipeId is null)
            throw new RecipeNotSelectedOnRequestException(RequestId);
    }
}