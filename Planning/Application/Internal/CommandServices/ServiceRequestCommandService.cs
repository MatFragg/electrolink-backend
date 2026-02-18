﻿using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class ServiceRequestCommandService(
    IServiceRequestRepository requestRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ExternalAssetsService externalAssetsService,
    ExternalSubscriptionsService externalSubscriptionsService,
    ILogger<ServiceRequestCommandService> logger)
    : IServiceRequestCommandService
{
    public async Task<ServiceRequest?> Handle(InitiateServiceRequestCommand command)
    {
        // Verificar elegibilidad via ACL a Subscriptions BC
        var (canCreate, reason) = await externalSubscriptionsService.CanCreateRequestAsync(command.HomeownerId);
        
        if (!canCreate)
        {
            logger.LogWarning($"[Planning BC] Homeowner {command.HomeownerId} cannot create request: {reason}");
            throw new InvalidOperationException(reason ?? "Cannot create request");
        }
        
        var request = ServiceRequest.Initiate(new HomeownerId(command.HomeownerId));
        await requestRepository.AddAsync(request);
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service request initiated: {request.Id.Id}");
        return request;
    }
    
    public async Task<ServiceRequest?> Handle(SelectPropertyForRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(new RequestId(command.RequestId))
            ?? throw new ArgumentException("Request not found");
        
        // Validar ownership
        if (request.HomeownerId.Id != command.HomeownerId)
            throw new UnauthorizedAccessException();
        
        // Obtener PropertySnapshot via ACL a Assets BC
        var propertySnapshot = await externalAssetsService.GetPropertySnapshotAsync(command.PropertyId);
        
        if (propertySnapshot == null)
            throw new ArgumentException($"Property {command.PropertyId} not found");
        
        request.SelectProperty(propertySnapshot);
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Property selected for request: {request.Id.Id}");
        return request;
    }
    
    public async Task<ServiceRequest?> Handle(AddServiceDetailsCommand command)
    {
        var request = await requestRepository.FindByIdAsync(new RequestId(command.RequestId))
            ?? throw new ArgumentException("Request not found");
        
        // Validar ownership
        if (request.HomeownerId.Id != command.HomeownerId)
            throw new UnauthorizedAccessException();
        
        // Verificar si usuario es premium via ACL a Subscriptions BC
        bool isPremiumUser = await externalSubscriptionsService.CanMarkAsPriorityAsync(command.HomeownerId);
        
        // Convertir DTOs a VOs
        var receiptData = new ReceiptData(
            command.ReceiptData.ConsumptionKwh,
            new Money(command.ReceiptData.AmountPaid, command.ReceiptData.Currency),
            command.ReceiptData.BillingPeriod,
            command.ReceiptData.ReceiptNumber
        );
        
        var preferences = new RequestPreferences(
            command.Preferences.PreferredDates,
            command.Preferences.TimePreference,
            command.Preferences.ProblemDescription
        );
        
        request.AddServiceDetails(
            new RecipeId(command.SelectedRecipeId),
            new TechnicianId(command.SelectedTechnicianId),
            receiptData,
            preferences,
            command.IsPriority,
            isPremiumUser
        );
        
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service details added for request: {request.Id.Id}");
        return request;
    }
    
    public async Task<ServiceRequest?> Handle(ConfirmServiceRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(new RequestId(command.RequestId))
            ?? throw new ArgumentException("Request not found");
        
        // Validar ownership
        if (request.HomeownerId.Id != command.HomeownerId)
            throw new UnauthorizedAccessException();
        
        request.Confirm();
        await unitOfWork.CompleteAsync();
        
        logger.LogInformation($"[Planning BC] Publishing {request.DomainEvents.Count} domain event(s) after request confirmation.");
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service request confirmed: {request.Id.Id}");
        return request;
    }
    
    public async Task<bool> Handle(CancelServiceRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(new RequestId(command.RequestId))
            ?? throw new ArgumentException("Request not found");
        
        // Validar ownership
        if (request.HomeownerId.Id != command.HomeownerId)
            throw new UnauthorizedAccessException();
        
        request.Cancel(command.Reason);
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        request.ClearDomainEvents();
        
        logger.LogInformation($"[Planning BC] Service request cancelled: {request.Id.Id}");
        return true;
    }
}


