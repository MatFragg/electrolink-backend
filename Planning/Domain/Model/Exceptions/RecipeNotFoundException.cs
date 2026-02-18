namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class RecipeNotFoundException : Exception
{
    public RecipeNotFoundException(Guid recipeId)
        : base($"Recipe with ID {recipeId} not found") { }
}

