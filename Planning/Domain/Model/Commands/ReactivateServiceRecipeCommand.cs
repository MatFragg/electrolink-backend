namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record ReactivateServiceRecipeCommand(
    string RecipeId,
    string TechnicianId
);
