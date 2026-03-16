using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record DeactivateComponentTypeCommand(ComponentTypeId ComponentTypeId);