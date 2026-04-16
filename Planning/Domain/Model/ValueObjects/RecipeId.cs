using System.Text.Json.Serialization;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RecipeId
{
    public string Value { get; }
    
    public RecipeId() {}

    [JsonConstructor]
    private RecipeId(string value) => Value = value;

    public static RecipeId NewId() => new($"recipe-{Guid.NewGuid()}");

    public static RecipeId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
             throw new ArgumentException("RecipeId cannot be empty");
             
        return new RecipeId(value);
    }
    
    public override string ToString() => Value;
}
