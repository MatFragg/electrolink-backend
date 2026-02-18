using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public partial class ServiceRequest
{
    // Wizard Step 2: Select Property
    public void SelectProperty(PropertySnapshot propertySnapshot)
    {
        if (Status != RequestStatus.Draft)
            throw new InvalidRequestStateException($"Cannot select property in status {Status}");
        
        if (propertySnapshot == null)
            throw new ArgumentNullException(nameof(propertySnapshot));
        
        PropertySnapshot = propertySnapshot;
        Status = RequestStatus.PropertySelected;
        
        _domainEvents.Add(new PropertySelectedForRequestEvent(
            Id.Id,
            propertySnapshot.PropertyId,
            propertySnapshot.Address,
            DateTime.UtcNow
        ));
    }
    
    // Wizard Step 4: Add Service Details
    public void AddServiceDetails(
        RecipeId recipeId,
        TechnicianId technicianId,
        ReceiptData receiptData,
        RequestPreferences preferences,
        bool isPriority,
        bool isPremiumUser)
    {
        if (Status != RequestStatus.PropertySelected)
            throw new InvalidRequestStateException($"Cannot add service details in status {Status}");
        
        if (isPriority && !isPremiumUser)
            throw new PriorityRequiresPremiumException();
        
        SelectedRecipeId = recipeId;
        SelectedTechnicianId = technicianId;
        ReceiptData = receiptData;
        Preferences = preferences;
        IsPriority = isPriority;
        Status = RequestStatus.ReadyToConfirm;
        
        _domainEvents.Add(new ServiceDetailsAddedEvent(
            Id.Id,
            recipeId.Id,
            technicianId.Id,
            isPriority,
            DateTime.UtcNow
        ));
    }
    
    // Wizard Step 5: Confirm
    public void Confirm()
    {
        if (Status != RequestStatus.ReadyToConfirm)
            throw new InvalidRequestStateException($"Cannot confirm in status {Status}");
        
        if (ReceiptData == null)
            throw new InvalidOperationException("Receipt data is required before confirming");
        
        Status = RequestStatus.PendingAssignment;
        
        _domainEvents.Add(new ServiceRequestCreatedEvent(
            Id.Id,
            HomeownerId.Id,
            PropertySnapshot!.PropertyId,
            PropertySnapshot.Geolocation.Latitude,
            PropertySnapshot.Geolocation.Longitude,
            SelectedRecipeId!.Id,
            SelectedTechnicianId!.Id,
            IsPriority,
            Preferences!.PreferredDates,
            Preferences.TimePreference,
            DateTime.UtcNow
        ));
    }
    
    // Cancel request
    public void Cancel(string reason)
    {
        if (Status == RequestStatus.Assigned)
            throw new InvalidOperationException("Cannot cancel an assigned request. Cancel the service instead.");
        
        if (Status == RequestStatus.Cancelled)
            throw new InvalidOperationException("Request is already cancelled.");
        
        Status = RequestStatus.Cancelled;
        CancellationReason = reason;
        
        _domainEvents.Add(new ServiceRequestCancelledEvent(
            Id.Id,
            reason,
            DateTime.UtcNow
        ));
    }
    
    // Mark as assigned (called by event handler after assignment)
    public void MarkAsAssigned(ServiceId serviceId)
    {
        if (Status != RequestStatus.PendingAssignment)
            throw new InvalidRequestStateException($"Cannot mark as assigned in status {Status}");
        
        AssignedServiceId = serviceId;
        Status = RequestStatus.Assigned;
        
        _domainEvents.Add(new ServiceRequestStatusChangedEvent(
            Id.Id,
            RequestStatus.PendingAssignment.ToString(),
            RequestStatus.Assigned.ToString(),
            DateTime.UtcNow
        ));
    }
}

