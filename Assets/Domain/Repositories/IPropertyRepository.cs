using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface IPropertyRepository : IBaseRepository<Property, PropertyId>
{
    Task<IEnumerable<Property>> FindByHomeownerIdAsync(HomeownerId homeownerId);
    Task<Property?> FindByIdAndOwnerIdAsync(PropertyId propertyId, HomeownerId homeownerId);
    
    Task<IEnumerable<Property>> GetAllFilteredAsync(
        HomeownerId ownerId, 
        string? city, 
        string? street
    );

    Task<(IEnumerable<Property> Items, int TotalCount)> GetAllPaginatedAsync(int page, int pageSize);

    Task<(IEnumerable<Property> Items, int TotalCount)> GetAllFilteredPaginatedAsync(
        HomeownerId ownerId, string? city, string? street, int page, int pageSize);
}