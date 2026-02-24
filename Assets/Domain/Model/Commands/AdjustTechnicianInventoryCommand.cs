using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record AdjustTechnicianInventoryCommand(string TechnicianId, List<ComponentAdjustment> Adjustments);