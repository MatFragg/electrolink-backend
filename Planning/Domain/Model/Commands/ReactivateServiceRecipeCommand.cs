namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record ReactivateServiceRecipeCommand(
    Guid RecipeId,
    Guid TechnicianId
);

