using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentStockCommand(TechnicianId TechnicianId,ComponentId ComponentId, int NewQuantity, int NewAlertThreshold);