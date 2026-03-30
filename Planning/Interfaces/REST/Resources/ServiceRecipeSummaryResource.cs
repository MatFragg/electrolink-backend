namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record ServiceRecipeSummaryResource(
    string RecipeId,
    string ServiceName,
    string ServiceCategory,
    decimal TotalPrice,
    string Currency,
    int EstimatedDurationMinutes,
    bool IsActive,
    int TimesRequested);