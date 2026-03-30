namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class RecipeNotFoundException : Exception
{
    public RecipeNotFoundException(string recipeId)
        : base($"Recipe with ID {recipeId} not found") { }
}

