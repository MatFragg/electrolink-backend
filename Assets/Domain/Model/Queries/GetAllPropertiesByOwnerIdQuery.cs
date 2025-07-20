namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

/// <summary>
/// Query to retrieve all properties that belong to a specific owner.
/// </summary>
public record GetAllPropertiesByOwnerIdQuery(
    Guid OwnerId, 
    string? City, 
    string? District, 
    string? Region, 
    string? Street
);