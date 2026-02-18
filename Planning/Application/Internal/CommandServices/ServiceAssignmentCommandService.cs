﻿using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class ServiceAssignmentCommandService(
    IServiceAssignmentRepository assignmentRepository,
    IServiceRequestRepository requestRepository,
    IServiceCatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ExternalAssetsService externalAssetsService,
    ILogger<ServiceAssignmentCommandService> logger)
    : IServiceAssignmentCommandService
{
    public async Task<ServiceAssignment?> Handle(AssignServiceToTechnicianCommand command)
    {
        // 1. Obtener request
        var request = await requestRepository.FindByIdAsync(new RequestId(command.RequestId))
            ?? throw new ArgumentException("Request not found");
        
        if (request.Status != RequestStatus.PendingAssignment)
            throw new InvalidOperationException($"Request must be in PendingAssignment status, current: {request.Status}");
        
        // 2. Obtener recipe snapshot
        var catalog = await catalogRepository.FindByTechnicianIdAsync(new TechnicianId(command.TechnicianId))
            ?? throw new InvalidOperationException("Technician catalog not found");
        
        var recipe = catalog.Recipes.FirstOrDefault(r => r.Id.Id == command.RecipeId)
            ?? throw new InvalidOperationException("Recipe not found in catalog");
        
        if (!recipe.IsActive)
            throw new InvalidOperationException("Recipe is not active");
        
        var recipeSnapshot = recipe.ToSnapshot();
        
        // 3. Verificar stock de componentes via ACL a Assets BC
        var hasStock = await externalAssetsService.CheckStockAvailabilityAsync(
            command.TechnicianId,
            recipeSnapshot.ComponentRequirements.ToList()
        );
        
        if (!hasStock)
        {
            logger.LogWarning($"[Planning BC] Technician {command.TechnicianId} does not have sufficient stock for recipe {command.RecipeId}");
            throw new InvalidOperationException("Technician does not have sufficient component stock");
        }
        
        // 4. Buscar slot disponible (simplificado por ahora - en el futuro usar MatchingAlgorithmService)
        var scheduledSlot = new ScheduledSlot(
            DateTime.UtcNow.AddDays(3),
            DateTime.UtcNow.AddDays(3).AddHours(recipeSnapshot.EstimatedDuration.TotalMinutes / 60.0)
        );
        
        // 5. Crear assignment
        var assignment = ServiceAssignment.Create(
            new RequestId(command.RequestId),
            new TechnicianId(command.TechnicianId),
            request.HomeownerId,
            new PropertyId(request.PropertySnapshot!.PropertyId),
            recipeSnapshot,
            scheduledSlot,
            command.IsPriority
        );
        
        await assignmentRepository.AddAsync(assignment);
        
        // 6. Marcar request como assigned
        request.MarkAsAssigned(assignment.Id);
        
        await unitOfWork.CompleteAsync();
        
        // 7. Reservar componentes en Assets BC
        await externalAssetsService.ReserveComponentsAsync(
            assignment.Id.Id,
            command.TechnicianId,
            recipeSnapshot.ComponentRequirements.ToList()
        );
        
        // 8. Publicar eventos de assignment
        logger.LogInformation($"[Planning BC] Publishing {assignment.DomainEvents.Count} domain event(s) after service assignment.");
        foreach (var domainEvent in assignment.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        assignment.ClearDomainEvents();
        
        // 9. Publicar eventos de request
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service assigned: {assignment.Id.Id} for request {command.RequestId}");
        return assignment;
    }
}


