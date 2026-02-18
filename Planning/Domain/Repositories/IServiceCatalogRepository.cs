﻿using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceCatalogRepository
{
    Task<ServiceCatalog?> FindByIdAsync(CatalogId catalogId);
    Task<ServiceCatalog?> FindByTechnicianIdAsync(TechnicianId technicianId);
    Task<IEnumerable<ServiceCatalog>> FindAllAsync();
    Task AddAsync(ServiceCatalog catalog);
    void Update(ServiceCatalog catalog);
}


