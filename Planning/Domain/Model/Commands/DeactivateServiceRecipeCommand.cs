namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record DeactivateServiceRecipeCommand(
    Guid RecipeId,
    Guid TechnicianId,
    string Reason
);

