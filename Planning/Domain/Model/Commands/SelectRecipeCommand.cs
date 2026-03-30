using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record SelectRecipeCommand(
    string RequestId,
    string RecipeId,
    string TechnicianId
);

