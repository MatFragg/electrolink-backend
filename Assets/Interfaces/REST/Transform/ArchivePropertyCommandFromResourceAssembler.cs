using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class ArchivePropertyCommandFromResourceAssembler
{
    public static ArchivePropertyCommand ToCommandFromResource(string propertyId, string reason)
        => new ArchivePropertyCommand(PropertyId.From(propertyId), reason);
}