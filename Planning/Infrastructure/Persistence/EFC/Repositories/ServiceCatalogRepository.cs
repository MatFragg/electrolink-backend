﻿using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ServiceCatalogRepository : BaseRepository<ServiceCatalog, CatalogId>, IServiceCatalogRepository
{
    public ServiceCatalogRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ServiceCatalog?> FindByIdAsync(CatalogId catalogId)
    {
        return await Context.Set<ServiceCatalog>()
            .Include(c => c.Recipes)
            .FirstOrDefaultAsync(c => c.Id == catalogId);
    }

    public async Task<ServiceCatalog?> FindByTechnicianIdAsync(TechnicianId technicianId)
    {
        return await Context.Set<ServiceCatalog>()
            .Include(c => c.Recipes)
            .FirstOrDefaultAsync(c => c.TechnicianId == technicianId);
    }

    public async Task<IEnumerable<ServiceCatalog>> FindAllAsync()
    {
        return await Context.Set<ServiceCatalog>()
            .Include(c => c.Recipes)
            .ToListAsync();
    }

    public new async Task AddAsync(ServiceCatalog catalog)
    {
        await Context.Set<ServiceCatalog>().AddAsync(catalog);
    }

    public void Update(ServiceCatalog catalog)
    {
        Context.Set<ServiceCatalog>().Update(catalog);
    }
}


