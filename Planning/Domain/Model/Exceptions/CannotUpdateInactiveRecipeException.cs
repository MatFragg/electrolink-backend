using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CannotUpdateInactiveRecipeException : DomainException
{
    public CannotUpdateInactiveRecipeException(string recipeId)
        : base($"Cannot update recipe {recipeId} because it is inactive.") { }

    public CannotUpdateInactiveRecipeException(RecipeId recipeId)
        : base($"Cannot update recipe {recipeId.Value} because it is inactive.") { }
}

