namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

/*public record AvailableServiceResource(
    string RecipeId,
    string TechnicianId,
    string FullName,
    double Rating,
    string ServiceName,
    string ServiceDescription,
    decimal TotalPrice,
    int EstimatedDurationMinutes,
    bool HasStock
);*/

public record AvailableServiceResource(
    string ServiceCategory,
    string CategoryDisplayName,
    decimal MinPrice,
    decimal MaxPrice,
    int EstimatedDurationMinutes,
    int TechniciansAvailable  
);
