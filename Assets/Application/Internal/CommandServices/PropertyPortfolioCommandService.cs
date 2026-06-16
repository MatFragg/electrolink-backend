using Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class PropertyPortfolioCommandService(
    IPropertyPortfolioRepository portfolioRepository,
    IPropertyRepository propertyRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ILogger<PropertyPortfolioCommandService> logger)
    : IPropertyPortfolioCommandService
{
    public async Task<PropertyPortfolio?> Handle(CreatePropertyPortfolioCommand command)
    {
        logger.LogInformation("[Assets BC] Creating portfolio for owner {OwnerId}", command.HomeownerId);

        // Idempotencia: si ya existe, no crear uno nuevo
        if (await portfolioRepository.FindByOwnerIdAsync(command.HomeownerId) is not null)
            throw new DuplicateAssetException("PropertyPortfolio", $"owner '{command.HomeownerId}'");

        var portfolio = PropertyPortfolio.Create(command.HomeownerId);

        await portfolioRepository.AddAsync(portfolio);
        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(portfolio);

        logger.LogInformation("[Assets BC] Portfolio {PortfolioId} created for owner {HomeownerId}",
            portfolio.Id, command.HomeownerId);

        return portfolio;
    }

    public async Task<PropertyPortfolio?> Handle(AddPropertyToPortfolioCommand command)
    {
        var portfolio = await portfolioRepository.FindByOwnerIdWithEntriesAsync(command.HomeownerId)
            ?? throw new AssetNotFoundException("PropertyPortfolio", $"owner '{command.HomeownerId}'");

        // Validación de ownership: la propiedad debe pertenecer al mismo owner
        var property = await propertyRepository.FindByIdAsync(command.PropertyId)
            ?? throw new AssetNotFoundException("Property", command.PropertyId.Value);

        if (property.OwnerId != command.HomeownerId)
            throw new UnauthorizedAccessException(
                $"Property {command.PropertyId} does not belong to owner {command.HomeownerId}.");

        portfolio.AddProperty(
            command.PropertyId,
            command.Nickname,
            command.IsPrimary,
            Enum.Parse<EOccupancyStatus>(command.OccupancyStatus, ignoreCase: true));
        property.MarkAsInPortfolio();

        portfolioRepository.Update(portfolio);
        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(portfolio);

        logger.LogInformation("[Assets BC] Property {PropertyId} added to portfolio of owner {OwnerId}",
            command.PropertyId, command.HomeownerId);

        return portfolio;
    }

    public async Task<bool> Handle(RemovePropertyFromPortfolioCommand command)
    {
        var portfolio = await portfolioRepository.FindByOwnerIdWithEntriesAsync(command.HomeownerId)
            ?? throw new AssetNotFoundException("PropertyPortfolio", $"owner '{command.HomeownerId}'");

        portfolio.RemoveProperty(command.PropertyId, command.Reason);
        
        var property = await propertyRepository.FindByIdAsync(command.PropertyId);
        property?.MarkAsAvailable();

        portfolioRepository.Update(portfolio);
        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(portfolio);

        logger.LogInformation("[Assets BC] Property {PropertyId} removed from portfolio of owner {OwnerId}. Reason: {Reason}",
            command.PropertyId, command.HomeownerId, command.Reason);

        return true;
    }

    private async Task PublishAndClearEventsAsync(PropertyPortfolio portfolio)
    {
        foreach (var domainEvent in portfolio.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        portfolio.ClearDomainEvents();
    }
}