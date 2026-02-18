using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceRequestCommandFromResourceAssemblers
{
    public static AddServiceDetailsCommand ToCommandFromResource(
        AddServiceDetailsResource resource,
        Guid requestId,
        Guid homeownerId)
    {
        if (!Enum.TryParse<TimePreference>(resource.Preferences.TimePreference, true, out var timePreference))
            throw new ArgumentException($"Invalid time preference: {resource.Preferences.TimePreference}");

        return new AddServiceDetailsCommand(
            requestId,
            homeownerId,
            resource.SelectedRecipeId,
            resource.SelectedTechnicianId,
            new ReceiptDataDto(
                resource.ReceiptData.ConsumptionKwh,
                resource.ReceiptData.AmountPaid,
                resource.ReceiptData.Currency,
                resource.ReceiptData.BillingPeriod,
                resource.ReceiptData.ReceiptNumber
            ),
            resource.IsPriority,
            new RequestPreferencesDto(
                resource.Preferences.PreferredDates,
                timePreference,
                resource.Preferences.ProblemDescription
            )
        );
    }
}

