using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceRequestResourceFromEntityAssembler
{
    public static ServiceRequestResource ToResourceFromEntity(ServiceRequest entity)
    {
        return new ServiceRequestResource(
            entity.Id.Id,
            entity.HomeownerId.Id,
            entity.Status.ToString(),
            entity.IsPriority,
            entity.PropertySnapshot != null ? new PropertySnapshotResource(
                entity.PropertySnapshot.PropertyId,
                entity.PropertySnapshot.Address,
                entity.PropertySnapshot.Geolocation.Latitude,
                entity.PropertySnapshot.Geolocation.Longitude
            ) : null,
            entity.SelectedRecipeId?.Id,
            entity.SelectedTechnicianId?.Id,
            entity.ReceiptData != null ? new ReceiptDataResource(
                entity.ReceiptData.ConsumptionKwh,
                entity.ReceiptData.AmountPaid.Amount,
                entity.ReceiptData.AmountPaid.Currency,
                entity.ReceiptData.BillingPeriod,
                entity.ReceiptData.ReceiptNumber
            ) : null,
            entity.Preferences != null ? new RequestPreferencesResource(
                entity.Preferences.PreferredDates.ToList(),
                entity.Preferences.TimePreference.ToString(),
                entity.Preferences.ProblemDescription
            ) : null,
            entity.AssignedServiceId?.Id,
            entity.CancellationReason
        );
    }

    public static ServiceRequestListResource ToListResourceFromEntity(
        ServiceRequest entity,
        string? serviceName = null)
    {
        return new ServiceRequestListResource(
            entity.Id.Id,
            entity.HomeownerId.Id,
            entity.Status.ToString(),
            entity.IsPriority,
            entity.PropertySnapshot?.Address,
            entity.SelectedRecipeId?.Id,
            serviceName
        );
    }
}

