﻿using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class ServiceCatalogCommandService(
    IServiceCatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ExternalAssetsService externalAssetsService,
    ILogger<ServiceCatalogCommandService> logger)
    : IServiceCatalogCommandService
{
    public async Task<ServiceCatalog?> Handle(CreateServiceCatalogCommand command)
    {
        // Validar que el técnico no tenga ya un catálogo
        var existing = await catalogRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId.Id));
        if (existing != null)
            throw new InvalidOperationException("Technician already has a catalog");
        
        var catalog = new ServiceCatalog(command.TechnicianId);
        await catalogRepository.AddAsync(catalog);
        await unitOfWork.CompleteAsync();
        
        // Publicar eventos de dominio
        logger.LogInformation($"[Planning BC] Publishing {catalog.DomainEvents.Count} domain event(s) after catalog creation.");
        foreach (var domainEvent in catalog.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        catalog.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service catalog created for technician {command.TechnicianId.Id}");
        return catalog;
    }
    
    public async Task<ServiceRecipe?> Handle(CreateServiceRecipeCommand command)
    {
        var catalog = await catalogRepository.FindByIdAsync(new CatalogId(command.CatalogId))
            ?? throw new CatalogNotFoundException(command.CatalogId);
        
        // Validar ownership
        if (catalog.TechnicianId.Id != command.TechnicianId)
            throw new UnauthorizedAccessException("Technician does not own this catalog");
        
        // Validar que componentes existen en Assets BC via ACL
        foreach (var componentReq in command.ComponentRequirements)
        {
            var exists = await externalAssetsService.ValidateComponentTypeExistsAsync(componentReq.ComponentTypeId);
            if (!exists)
            {
                logger.LogWarning($"[Planning BC] Component type {componentReq.ComponentTypeId} not found in Assets BC");
                throw new ArgumentException($"Component type {componentReq.ComponentTypeName} does not exist");
            }
        }
        
        var recipe = catalog.AddRecipe(command);
        await unitOfWork.CompleteAsync();
        
        // Publicar eventos
        logger.LogInformation($"[Planning BC] Publishing {catalog.DomainEvents.Count} domain event(s) after recipe creation.");
        foreach (var domainEvent in catalog.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        catalog.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service recipe created: {recipe.Id.Id}");
        return recipe;
    }
    
    public async Task<ServiceRecipe?> Handle(UpdateServiceRecipeCommand command)
    {
        var catalog = await catalogRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId))
            ?? throw new CatalogNotFoundException(Guid.Empty);
        
        // Validar ownership
        if (catalog.TechnicianId.Id != command.TechnicianId)
            throw new UnauthorizedAccessException("Technician does not own this catalog");
        
        catalog.UpdateRecipe(new RecipeId(command.RecipeId), command);
        await unitOfWork.CompleteAsync();
        
        // Publicar eventos
        foreach (var domainEvent in catalog.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        catalog.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service recipe updated: {command.RecipeId}");
        return catalog.Recipes.FirstOrDefault(r => r.Id.Id == command.RecipeId);
    }
    
    public async Task<bool> Handle(DeactivateServiceRecipeCommand command)
    {
        var catalog = await catalogRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId))
            ?? throw new CatalogNotFoundException(Guid.Empty);
        
        // Validar ownership
        if (catalog.TechnicianId.Id != command.TechnicianId)
            throw new UnauthorizedAccessException("Technician does not own this catalog");
        
        catalog.DeactivateRecipe(new RecipeId(command.RecipeId), command.Reason);
        await unitOfWork.CompleteAsync();
        
        // Publicar eventos
        foreach (var domainEvent in catalog.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        catalog.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service recipe deactivated: {command.RecipeId}");
        return true;
    }
    
    public async Task<bool> Handle(ReactivateServiceRecipeCommand command)
    {
        var catalog = await catalogRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId))
            ?? throw new CatalogNotFoundException(Guid.Empty);
        
        // Validar ownership
        if (catalog.TechnicianId.Id != command.TechnicianId)
            throw new UnauthorizedAccessException("Technician does not own this catalog");
        
        catalog.ReactivateRecipe(new RecipeId(command.RecipeId));
        await unitOfWork.CompleteAsync();
        
        // Publicar eventos
        foreach (var domainEvent in catalog.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        catalog.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service recipe reactivated: {command.RecipeId}");
        return true;
    }
}


