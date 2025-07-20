using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface IPropertyRepository : IBaseRepository<Property>
{
    Task<Property?> FindByIdAsync(PropertyId id);

    Task<Property?> FindByIdAndOwnerIdAsync(PropertyId propertyId, OwnerId ownerId);
    
    Task<IEnumerable<Property>> GetAllFilteredAsync(
        OwnerId ownerId, 
        string? city, 
        string? district, 
        string? region, 
        string? street
    );
}