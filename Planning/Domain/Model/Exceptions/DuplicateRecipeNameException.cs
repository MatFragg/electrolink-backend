namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class DuplicateRecipeNameException : Exception
{
    public DuplicateRecipeNameException(string recipeName)
        : base($"A recipe with name '{recipeName}' already exists in this catalog") { }
}

