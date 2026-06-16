using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class PropertyRepository(AppDbContext context) : BaseRepository<Property, PropertyId>(context), IPropertyRepository
{
    public async Task<IEnumerable<Property>> FindByHomeownerIdAsync(HomeownerId homeownerId)
    {
        return await Context.Set<Property>()
            .Where(p => p.OwnerId == homeownerId)
            .ToListAsync();
    }
    
    public async Task<Property?> FindByIdAndOwnerIdAsync(PropertyId propertyId, HomeownerId ownerId)
    {
        return await Context.Set<Property>()
            .FirstOrDefaultAsync(p => p.Id == propertyId && p.OwnerId == ownerId);
    }
    public async Task<IEnumerable<Property>> GetAllFilteredAsync(HomeownerId ownerId,
        string? city,
        string? street)
    {
        var query = Context.Set<Property>().Where(p => p.OwnerId == ownerId);

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(p => p.Address.City.Contains(city));
        }
        if (!string.IsNullOrWhiteSpace(street))
        {
            query = query.Where(p => p.Address.Street.Contains(street));
        }
        
        return await query.ToListAsync();
    }

    public async Task<(IEnumerable<Property> Items, int TotalCount)> GetAllPaginatedAsync(int page, int pageSize)
    {
        var query = Context.Set<Property>();
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }

    public async Task<(IEnumerable<Property> Items, int TotalCount)> GetAllFilteredPaginatedAsync(
        HomeownerId ownerId, string? city, string? street, int page, int pageSize)
    {
        var query = Context.Set<Property>().Where(p => p.OwnerId == ownerId);

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(p => p.Address.City.Contains(city));

        if (!string.IsNullOrWhiteSpace(street))
            query = query.Where(p => p.Address.Street.Contains(street));

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }
}
