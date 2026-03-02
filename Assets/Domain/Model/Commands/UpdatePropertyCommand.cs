using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdatePropertyCommand(PropertyId PropertyId, HomeownerId HomeownerId, Address Address);