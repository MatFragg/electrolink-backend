using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record RemoveComponentStockCommand(TechnicianId TechnicianId, ComponentId ComponentId);