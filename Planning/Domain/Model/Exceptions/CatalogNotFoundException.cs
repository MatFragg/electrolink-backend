namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CatalogNotFoundException : Exception
{
    public CatalogNotFoundException(Guid catalogId)
        : base($"Catalog with ID {catalogId} not found") { }
}

