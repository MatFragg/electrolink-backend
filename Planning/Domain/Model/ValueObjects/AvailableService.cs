namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

/*public record AvailableService(
    string RecipeId,
    string TechnicianId,
    string FullName,
    double Rating,
    string ServiceName,
    string ServiceDescription,
    decimal TotalPrice,
    int EstimatedDurationMinutes,
    bool HasStock);*/
    
public record AvailableService(
    string ServiceCategory,
    string CategoryDisplayName,
    decimal MinPrice,
    decimal MaxPrice,
    int EstimatedDurationMinutes,
    int TechniciansAvailable);