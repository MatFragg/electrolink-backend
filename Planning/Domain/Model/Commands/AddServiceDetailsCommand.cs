namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record AddServiceDetailsCommand(
    Guid RequestId,
    Guid HomeownerId,
    Guid SelectedRecipeId,
    Guid SelectedTechnicianId,
    ReceiptDataDto ReceiptData,
    bool IsPriority,
    RequestPreferencesDto Preferences
);

