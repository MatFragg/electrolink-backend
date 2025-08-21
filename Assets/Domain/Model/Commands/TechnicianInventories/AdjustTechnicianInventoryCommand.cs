using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;

public record AdjustTechnicianInventoryCommand(Guid TechnicianId, List<ComponentAdjustment> Adjustments);