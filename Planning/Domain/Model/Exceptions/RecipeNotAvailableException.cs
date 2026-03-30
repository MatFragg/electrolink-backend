namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class RecipeNotAvailableException : Exception
{
    public RecipeNotAvailableException(string message) : base(message)
    {
    }
}