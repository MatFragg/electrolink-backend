namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RecipeId(Guid Id)
{
    public RecipeId() : this(Guid.Empty) { }
    public static RecipeId NewId() => new(Guid.NewGuid());
}

