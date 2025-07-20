namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

/// <summary>
/// Query to retrieve all associated photos to a specific property.
/// This query is used to fetch all photos linked to a property by its unique identifier.
/// </summary>
/// <param name="PropertyId"></param>
public record GetAllPhotosByPropertyIdQuery(Guid PropertyId);