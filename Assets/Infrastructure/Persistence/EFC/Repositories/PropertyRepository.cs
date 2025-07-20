using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class PropertyRepository(AppDbContext context) : BaseRepository<Property>(context), IPropertyRepository
{
    public async Task<IEnumerable<Property>> FindByOwnerIdAsync(OwnerId ownerId)
    {
        return await Context.Set<Property>()
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
    }
    
    public async Task<Property?> FindByIdAsync(PropertyId id)
    {
        return await Context.Set<Property>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<Property>> FindByCityAsync(string city)
    {
        // Implementar la lógica para buscar propiedades por ciudad
        // Por ejemplo:
        return await Context.Properties
            .Where(p => p.Address.City.Contains(city))
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> FindByStreetAsync(string street)
    {
        // Implementar la lógica para buscar propiedades por calle
        return await Context.Properties
            .Where(p => p.Address.Street.Contains(street))
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> FindByRegionAsync(string regionName)
    {
        // Implementar la lógica para buscar propiedades por región
        return await Context.Properties
            .Where(p => p.Region.Name == regionName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> FindByDistrictAsync(string districtName)
    {
        // Implementar la lógica para buscar propiedades por distrito
        return await Context.Properties
            .Where(p => p.District.Name == districtName)
            .ToListAsync();
    }
    
    /*public async Task<Property?> Handle(DeactivatePropertyCommand command)
    {
        var property = await FindByIdAsync(new PropertyId(command.Id));
        if (property == null) return null;
        
        property.Handle(command);
        await Context.SaveChangesAsync();
        return property;
    }*/
    
    public async Task<Property?> FindByIdAndOwnerIdAsync(PropertyId propertyId, OwnerId ownerId)
    {
        return await Context.Set<Property>()
            .FirstOrDefaultAsync(p => p.Id == propertyId && p.OwnerId == ownerId);
    }
    public async Task<IEnumerable<Property>> GetAllFilteredAsync(
        OwnerId ownerId, 
        string? city, 
        string? district, 
        string? region, 
        string? street)
    {
        var query = Context.Set<Property>().Where(p => p.OwnerId == ownerId);

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(p => p.Address.City.Contains(city));
        }
        if (!string.IsNullOrWhiteSpace(district))
        {
            query = query.Where(p => p.District.Name == district);
        }
        if (!string.IsNullOrWhiteSpace(region))
        {
            query = query.Where(p => p.Region.Name == region);
        }
        if (!string.IsNullOrWhiteSpace(street))
        {
            query = query.Where(p => p.Address.Street.Contains(street));
        }
        
        return await query.ToListAsync();
    }
}
