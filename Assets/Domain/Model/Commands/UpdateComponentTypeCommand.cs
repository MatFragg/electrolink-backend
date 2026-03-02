using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentTypeCommand(ComponentTypeId ComponentTypeId, string Name, string? Description);
