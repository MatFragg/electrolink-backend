namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class AtLeastOneComponentRequiredException : DomainException
{
    public AtLeastOneComponentRequiredException()
        : base("At least one component is required for a service recipe.") { }

    public AtLeastOneComponentRequiredException(string recipeId)
        : base($"Service recipe {recipeId} requires at least one component.") { }

    public AtLeastOneComponentRequiredException(Guid recipeId)
        : base($"Service recipe {recipeId} requires at least one component.") { }
}