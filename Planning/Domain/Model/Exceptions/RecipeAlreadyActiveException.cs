using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class RecipeAlreadyActiveException : DomainException
{
    public RecipeAlreadyActiveException(string recipeId)
        : base($"Recipe {recipeId} is already active.") { }

    public RecipeAlreadyActiveException(RecipeId recipeId)
        : base($"Recipe {recipeId.Value} is already active.") { }
}

