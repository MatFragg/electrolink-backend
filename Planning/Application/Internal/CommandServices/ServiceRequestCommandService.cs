using System.Globalization;
using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;
using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class ServiceRequestCommandService(
    IServiceRequestRepository requestRepository,
    IServiceCatalogRepository catalogRepository,
    IServiceAssignmentCommandService assignmentCommandService,
    ISubscriptionContextFacade subscriptionFacade,
    ExternalProfilesService externalProfilesService,
    IAssetsContextFacade assetsFacade,
    IUnitOfWork unitOfWork,
    ILogger<ServiceRequestCommandService> logger)
    : IServiceRequestCommandService
{
    public async Task<RequestId?> Handle(InitiateServiceRequestCommand command)
    {
        await externalProfilesService.EnsureHomeownerIsActiveAsync(command.HomeownerId.ToString());
        await externalProfilesService.EnsureHasPropertiesAsync(command.HomeownerId.ToString());

        var eligibility = await subscriptionFacade.GetRequestEligibilityAsync(command.HomeownerId.Value);

        if (!eligibility.canCreate)
            throw new RequestLimitReachedException(command.HomeownerId);

        var request = ServiceRequest.Initiate(
            command.HomeownerId,
            eligibility.canMarkAsPriority,
            eligibility.remainingRequests);

        await requestRepository.AddAsync(request);
        await unitOfWork.CompleteAsync();

        return request.RequestId;
    }

    public async Task<ServiceRequest?> Handle(SelectPropertyForRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId) ?? throw new InvalidOperationException($"ServiceRequest with ID {command.RequestId} not found.");

        EnsureOwnership(request.HomeownerId, command.HomeownerId);

        var geolocation = await assetsFacade.GetPropertyGeolocationAsync(
            command.PropertyId.Value, command.HomeownerId.Value) ;

        if (geolocation is null)
            throw new InvalidOperationException("Property geolocation data unavailable.");
        
        request.SelectProperty(command.PropertyId, Geolocation.Create(geolocation.Value.Latitude, geolocation.Value.Longitude, null, "MANUAL"));

        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();
        return request;
    }
    
    // For simplicity, we assume that the recipe details are static at the time of selection. In a real-world scenario, we might want to capture more dynamic details or handle cases where the recipe changes after selection. In addition, this command does not handle the assignment automatically, as that would typically be part of a separate workflow step after the recipe and technician is selected and the request is confirmed.
    /*public async Task<ServiceRequest?> Handle(SelectServiceRecipeCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId) ?? throw new InvalidOperationException($"ServiceRequest with ID {command.RequestId} not found.");

        EnsureOwnership(request.HomeownerId, command.HomeownerId);

        var catalog = await catalogRepository.FindByTechnicianIdAsync(command.TechnicianId) ?? throw new InvalidOperationException($"Service catalog for technician {command.TechnicianId} not found.");
        
        var recipe = catalog.Recipes.FirstOrDefault(r =>
                         r.Id == command.RecipeId && r.IsActive)
                     ?? throw new RecipeNotAvailableException(command.RecipeId.Value);

        var snapshot = RecipeSnapshot.FromRecipe(recipe);

        request.SelectRecipe(
            command.RecipeId,
            command.TechnicianId,
            snapshot);

        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();
        
        return request;
    }*/

    public async Task<ServiceRequest?> Handle(SelectServiceRecipeCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId)
                      ?? throw new InvalidOperationException($"ServiceRequest {command.RequestId} not found.");

        EnsureOwnership(request.HomeownerId, command.HomeownerId);

        request.SelectCategory(command.ServiceCategory);

        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();

        return request;
    }
    
    public async Task<ServiceRequest?> Handle(AddServiceDetailsCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId) ?? throw new InvalidOperationException($"ServiceRequest with ID {command.RequestId} not found.");

        EnsureOwnership(request.HomeownerId, command.HomeownerId);

        var currency = Enum.Parse<ECurrency>(command.AmountCurrency, ignoreCase: true);
        var dates = command.PreferredDates
            .Select(d => DateOnly.ParseExact(d, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            .ToList();

        var preferences = RequestPreferences.Create(
            command.ProblemDescription,
            command.ConsumptionKwh,
            Money.Of(command.AmountPaid, currency),
            command.BillingPeriod,
            command.ReceiptNumber,
            dates,
            Enum.Parse<ETimePreference>(command.TimePreference, ignoreCase: true));

        request.AddDetails(preferences, command.IsPriority);

        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();

        return request;
    }

    public async Task<ServiceRequest?> Handle(ConfirmServiceRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId) ?? throw new InvalidOperationException($"ServiceRequest with ID {command.RequestId} not found.");

        EnsureOwnership(request.HomeownerId, command.HomeownerId);

        var eligibility = await subscriptionFacade.GetRequestEligibilityAsync(command.HomeownerId.Value);
        if (!eligibility.canCreate)
            throw new RequestLimitReachedException(command.HomeownerId);

        request.Confirm();

        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();

        await assignmentCommandService.Handle(
            new ExecuteMatchingAlgorithmCommand(request.RequestId));
        
        return request;
    }

    public async Task<bool> Handle(CancelServiceRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId) ?? throw new InvalidOperationException($"ServiceRequest with ID {command.RequestId} not found.");

        EnsureOwnership(request.HomeownerId, command.HomeownerId);

        request.Cancel(CancellationReason.From(command.Reason), command.Notes);

        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();

        return true;
    }

    private static void EnsureOwnership(HomeownerId actual, HomeownerId expected)
    {
        if (actual != expected)
            throw new UnauthorizedRequestAccessException();
    }
}