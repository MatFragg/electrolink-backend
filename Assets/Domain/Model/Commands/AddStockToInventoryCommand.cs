using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AddStockToInventoryCommand(TechnicianId TechnicianId, ComponentId ComponentId, ComponentTypeId ComponentTypeId, int Quantity, int AlertThreshold);
