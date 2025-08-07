using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface ITechnicianInventoryRepository : IBaseRepository<TechnicianInventory>
{
    Task<TechnicianInventory?> FindByTechnicianIdAsync(TechnicianId technicianId);
}