using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class ComponentTypeRepository(AppDbContext context) 
    : BaseRepository<ComponentType, ComponentTypeId>(context), IComponentTypeRepository
{
    public async Task<bool> ExistsByNameAsync(string name) 
        => await Context.Set<ComponentType>().AnyAsync(ct => ct.Name == name);
    
    public async Task<bool> ExistsActiveByIdAsync(string id)
        => await Context.Set<ComponentType>()
            .AnyAsync(ct => ct.Id == ComponentTypeId.From(id) && ct.IsActive);

    public Task<string> FindComponentTypeNameByIdAsync(ComponentTypeId componentTypeId) 
        => Context.Set<ComponentType>()
            .Where(ct => ct.Id == componentTypeId)
            .Select(ct => ct.Name)
            .FirstOrDefaultAsync()!;
}