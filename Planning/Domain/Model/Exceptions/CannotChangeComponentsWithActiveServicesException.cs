using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CannotChangeComponentsWithActiveServicesException : DomainException
{
    public CannotChangeComponentsWithActiveServicesException(string recipeId, int activeCount)
        : base($"Cannot change components for recipe {recipeId} because there are {activeCount} active services.") { }

    public CannotChangeComponentsWithActiveServicesException(RecipeId recipeId, int activeCount)
        : base($"Cannot change components for recipe {recipeId.Value} because there are {activeCount} active services.") { }
}

