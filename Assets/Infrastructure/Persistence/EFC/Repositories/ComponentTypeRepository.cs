using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class ComponentTypeRepository(AppDbContext context) 
    : BaseRepository<ComponentType>(context), IComponentTypeRepository
{
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Set<ComponentType>().AnyAsync(ct => ct.Name == name);
    }
    
    public async Task<ComponentType?> FindByIdAsync(ComponentTypeId id)
    {
        return await Context.Set<ComponentType>().FirstOrDefaultAsync(ct => ct.Id == id);
    }
}