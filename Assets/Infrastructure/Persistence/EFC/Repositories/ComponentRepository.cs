using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class ComponentRepository(AppDbContext context) : BaseRepository<Component>(context), IComponentRepository
{
    public async Task<Component?> FindByIdAsync(ComponentId id)
    {
        return await Context.Set<Component>().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Component>> FindByTypeIdAsync(ComponentTypeId typeId)
    {
        return await Context.Set<Component>()
            .Where(c => c.TypeId == typeId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Set<Component>().AnyAsync(c => c.Name == name);
    }

    // --- MÉTODO FALTANTE AÑADIDO ---
    public async Task<IEnumerable<Component>> FindByIdsAsync(IEnumerable<ComponentId> ids)
    {
        // Convertimos la lista de Value Objects a una lista de Guids primitivos
        return await Context.Set<Component>()
            .Where(c => ids.Contains(c.Id)) // <-- Compara ComponentId con una lista de ComponentId.
            .ToListAsync();
    }
}