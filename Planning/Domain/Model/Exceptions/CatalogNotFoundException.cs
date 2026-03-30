using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class CatalogNotFoundException : Exception
{
    public CatalogNotFoundException(TechnicianId technicianId)
        : base($"Catalog with ID {technicianId} not found") { }
}

