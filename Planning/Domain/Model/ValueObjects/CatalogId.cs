namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record CatalogId(Guid Id)
{
    public CatalogId() : this(Guid.Empty) { }
    public static CatalogId NewId() => new(Guid.NewGuid());
}

