using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class PriceIncreaseTooLargeException : DomainException
{
    public PriceIncreaseTooLargeException(string recipeId)
        : base($"Price increase too large for recipe {recipeId}.") { }

    public PriceIncreaseTooLargeException(RecipeId recipeId)
        : base($"Price increase too large for recipe {recipeId.Value}.") { }
}

