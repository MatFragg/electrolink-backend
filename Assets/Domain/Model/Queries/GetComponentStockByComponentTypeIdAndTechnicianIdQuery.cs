using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

public record GetComponentStockByComponentTypeIdAndTechnicianIdQuery(ComponentTypeId ComponentTypeId, TechnicianId TechnicianId);