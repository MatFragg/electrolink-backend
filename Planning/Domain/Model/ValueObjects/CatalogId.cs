namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record CatalogId
{
    public string Value { get; }

    private CatalogId(string value) => Value = value;

    public static CatalogId NewId() => new($"cat-{Guid.NewGuid()}");

    public static CatalogId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
             throw new ArgumentException("CatalogId cannot be empty");
             
        return new CatalogId(value);
    }
    
    public override string ToString() => Value;
}
