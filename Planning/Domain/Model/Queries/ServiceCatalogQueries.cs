namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

// ServiceCatalog Queries
public record GetServiceCatalogByTechnicianQuery(Guid TechnicianId);

public record GetServiceRecipeByIdQuery(Guid RecipeId);

public record GetActiveRecipesByTechnicianQuery(Guid TechnicianId);

public record GetAllRecipesByCatalogIdQuery(Guid CatalogId);

