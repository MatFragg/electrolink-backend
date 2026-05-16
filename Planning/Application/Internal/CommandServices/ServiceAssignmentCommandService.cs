using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public class ServiceAssignmentCommandService(
    IServiceAssignmentRepository assignmentRepository,
    IServiceRequestRepository requestRepository,
    ExternalProfilesService externalProfilesService,
    ExternalAssetsService externalAssetsService,
    IServiceCatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IAIMatchingProvider aiMatchingProvider,
    ILogger<ServiceAssignmentCommandService> logger)
    : IServiceAssignmentCommandService
{
    public async Task Handle(ExecuteMatchingAlgorithmCommand command)
    {
        var request = await requestRepository.FindByIdAsync(RequestId.From(command.RequestId.Value)) ?? throw new InvalidOperationException($"ServiceRequest with ID {command.RequestId.Value} not found.");

        // Validación explícita antes de continuar
        if (request.Geolocation is null)
            throw new InvalidOperationException(
                $"ServiceRequest {command.RequestId.Value} has no geolocation. " +
                "The homeowner must select a property first.");

        if (request.RequestedCategory is null)
            throw new InvalidOperationException(
                $"ServiceRequest {command.RequestId.Value} has no category selected. " +
                "The homeowner must select a service category first.");
        
        var techniciansInArea = (await externalProfilesService.GetTechniciansInAreaAsync(
            request.Geolocation!.Latitude, request.Geolocation.Longitude)).ToList();

        var candidates = await FilterCandidatesAsync(techniciansInArea, request);

        if (!candidates.Any())
        {
            var existingRetries = await GetRetryCountAsync(request.RequestId);
            var failed = ServiceAssignment.Fail(
                request.RequestId, "NO_CANDIDATES_AVAILABLE", existingRetries);

            await assignmentRepository.AddAsync(failed);
            await unitOfWork.CompleteAsync();
        
            throw new NoCandidatesAvailableException("NO_CANDIDATES_AVAILABLE");
        }

        var best = await SelectBestCandidateAsync(candidates, request);

        var snapshot = RecipeSnapshot.FromRecipe(best.Recipe);

        var criteria = MatchingCriteria.Create(
            request.Geolocation,
            best.Recipe.ComponentRequirements.Select(c => c.ComponentTypeId).ToList(),
            request.IsPriority,
            TechnicianId.From(best.TechnicianId));

        var assignment = ServiceAssignment.Assign(
            request.RequestId,
            TechnicianId.From(best.TechnicianId),
            snapshot,
            criteria);

        request.MarkAsAssigned(
            assignment.AssignmentId,
            TechnicianId.From(best.TechnicianId),
            snapshot);
        
        foreach (var domainEvent in request.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }

        request.ClearDomainEvents();
        
        foreach (var domainEvent in assignment.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        assignment.ClearDomainEvents(); 

        await assignmentRepository.AddAsync(assignment);
        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();
    }

    /*
     * public async Task<bool> Handle(ExecuteMatchingAlgorithmCommand command)
    {
        var request = await requestRepository.FindByIdAsync(
            RequestId.From(command.RequestId.Value)) ?? throw new InvalidOperationException($"ServiceRequest with ID {command.RequestId.Value} not found.");

        var techniciansInArea = (await externalProfilesService.GetTechniciansInAreaAsync(
            request.Geolocation!.Latitude, request.Geolocation.Longitude)).ToList();

        var candidates = await FilterCandidatesAsync(techniciansInArea, request);

        if (!candidates.Any())
        {
            var existingRetries = await GetRetryCountAsync(request.RequestId);
            var failed = ServiceAssignment.Fail(
                request.RequestId, "NO_CANDIDATES_AVAILABLE", existingRetries);

            await assignmentRepository.AddAsync(failed);
            await unitOfWork.CompleteAsync();
            
            // Retorna false para indicar que NO fue asignado
            return false;
        }

        var best = SelectBestCandidate(candidates, request.IsPriority);
        var technicianId = TechnicianId.From(best.TechnicianId);

        var snapshot = best.Recipe;
        var criteria = MatchingCriteria.Create(
            request.Geolocation,
            best.Recipe.ComponentRequirements.Select(c => c.ComponentTypeId).ToList(),
            request.IsPriority,
            technicianId);

        var assignment = ServiceAssignment.Assign(
            request.RequestId,
            technicianId,
            snapshot,
            criteria);

        // Agregado el TechnicianId asumiendo que tu entidad de dominio lo necesita persisitir
        // Tendrás que ajustar la firma del método MarkAsAssigned en tu clase ServiceRequest.
        request.MarkAsAssigned(assignment.AssignmentId);

        await assignmentRepository.AddAsync(assignment);
        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();
        
        // Retorna true porque la asignación fue exitosa
        return true;
    }
     */
    
    // OG VERSION
    /*private async Task<IReadOnlyList<Candidate>> FilterCandidatesAsync(
        IReadOnlyList<(string technicianId, string profileId, string fullName, double rating)> technicians,
        ServiceRequest request)
    {
        var candidates = new List<Candidate>();

        // RecipeSnapshot ya está validado como no-null antes de llamar este método
        var componentRequirements = request.RecipeSnapshot.ComponentRequirements
            .Select(c => (c.ComponentTypeId, c.Quantity))
            .ToList();

        foreach (var tech in technicians)
        {
            var stockCheck = await assetsFacade.CheckAllComponentsInStockAsync(
                tech.technicianId,
                componentRequirements);  // reutiliza la lista, no recalcula por cada técnico

            if (!stockCheck) continue;

            candidates.Add(new Candidate(tech.technicianId, tech.rating, request.RecipeSnapshot));
        }

        return candidates;
    }*/

    private async Task<IReadOnlyList<Candidate>> FilterCandidatesAsync(
        IReadOnlyList<(string technicianId, string profileId, string fullName, double rating)> technicians,
        ServiceRequest request)
    {
        var candidates = new List<Candidate>();

        foreach (var tech in technicians)
        {
            // Busca el recipe activo del técnico para esa categoría
            var recipe = await catalogRepository.FindActiveRecipeByCategoryAndTechnicianAsync(request.RequestedCategory!.Value,
                TechnicianId.From(tech.technicianId));

            if (recipe is null) continue;

            // Verifica stock para ese recipe específico
            var componentRequirements = recipe.ComponentRequirements
                .Select(cr => (cr.ComponentTypeId, cr.Quantity))
                .ToList();

            var stockOk = await externalAssetsService.CheckComponentStockAsync(
                tech.technicianId,
                componentRequirements);

            if (!stockOk) continue;

            candidates.Add(new Candidate(tech.technicianId, tech.rating, recipe));
        }

        return candidates;
    }

    private async Task<Candidate> SelectBestCandidateAsync(IReadOnlyList<Candidate> candidates, ServiceRequest request)
    {
        var contextCandidates = candidates.Select(c => new TechnicianCandidate(
            c.TechnicianId,
            0.0, // We could calculate actual distance here if needed
            c.Rating,
            10, // Mock completed count
            true, // Mock IoT cert
            new List<string>(), // Mock specialties
            true,
            30 // Mock response time
        )).ToList();

        var context = new MatchingContext(
            request.RequestedCategory?.ToString() ?? "General",
            request.Preferences?.ProblemDescription ?? "",
            request.IsPriority,
            request.Geolocation!,
            contextCandidates,
            null
        );

        var scored = await aiMatchingProvider.ScoreTechnicianCandidatesAsync(context);

        if (scored.Any())
        {
            var topCandidateId = scored.OrderByDescending(s => s.Score).First().TechnicianId;
            var topCandidate = candidates.FirstOrDefault(c => c.TechnicianId == topCandidateId);
            if (topCandidate != null)
                return topCandidate;
        }

        return candidates.OrderByDescending(c => c.Rating).First();
    }

    private async Task<int> GetRetryCountAsync(RequestId requestId)
    {
        var existing = await assignmentRepository.FindByRequestIdAsync(requestId);
        return existing?.RetryCount ?? 0;
    }
}

internal record Candidate(string TechnicianId, double Rating, ServiceRecipe Recipe);
