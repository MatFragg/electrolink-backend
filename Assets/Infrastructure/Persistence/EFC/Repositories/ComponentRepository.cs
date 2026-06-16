using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class ComponentRepository(AppDbContext context) : BaseRepository<Component, ComponentId>(context), IComponentRepository
{
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Set<Component>().AnyAsync(c => c.Name == name);
    }

    public async Task<IEnumerable<Component>> FindByIdsAsync(IEnumerable<ComponentId> ids)
    {
        var idsList = ids.ToList();
        
        if (idsList.Count == 0) return new List<Component>();

        return await Context.Set<Component>()
            .Where(c => idsList.Contains(c.Id))
            .ToListAsync();
    }

    public async Task<(IEnumerable<Component> Items, int TotalCount)> GetAllPaginatedAsync(int page, int pageSize)
    {
        var query = Context.Set<Component>();
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }
}