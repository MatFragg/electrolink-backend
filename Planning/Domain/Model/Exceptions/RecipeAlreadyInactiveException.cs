using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class RecipeAlreadyInactiveException : DomainException
{
    public RecipeAlreadyInactiveException(string recipeId)
        : base($"Recipe {recipeId} is already inactive.") { }

    public RecipeAlreadyInactiveException(RecipeId recipeId)
        : base($"Recipe {recipeId.Value} is already inactive.") { }
}

