using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CannotDeactivateRecipeWithInProgressServicesException : DomainException
{
    public CannotDeactivateRecipeWithInProgressServicesException(string recipeId, int inProgressCount)
        : base($"Cannot deactivate recipe {recipeId} because there are {inProgressCount} in-progress services.") { }

    public CannotDeactivateRecipeWithInProgressServicesException(RecipeId recipeId, int inProgressCount)
        : base($"Cannot deactivate recipe {recipeId.Value} because there are {inProgressCount} in-progress services.") { }
}

