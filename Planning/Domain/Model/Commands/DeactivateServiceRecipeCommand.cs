using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record DeactivateServiceRecipeCommand(
    CatalogId CatalogId,
    RecipeId RecipeId,
    TechnicianId TechnicianId,
    string Reason,
    string? Notes = null);

