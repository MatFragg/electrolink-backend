using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

public record GetStockItemDetailsQuery(TechnicianId TechnicianId, ComponentId ComponentId);
