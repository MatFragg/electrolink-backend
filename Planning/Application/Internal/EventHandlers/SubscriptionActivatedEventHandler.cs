using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

public class SubscriptionActivatedEventHandler(
    IServiceCatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    ExternalProfilesService externalProfiles,
    ILogger<SubscriptionActivatedEventHandler> logger)
    : IEventHandler<SubscriptionActivatedEvent>
{
    public async Task Handle(SubscriptionActivatedEvent @event, CancellationToken cancellationToken)
    {
        if (@event.BusinessRole != "TECHNICIAN")
        {
            logger.LogInformation("[Planning] Ignored SubscriptionActivated for BusinessRole={Role}", @event.BusinessRole);
            return;
        }

        var technicianId = await externalProfiles.GetTechnicianIdByUserIdAsync(@event.UserId);

        if (string.IsNullOrWhiteSpace(technicianId))
        {
            logger.LogWarning("[Planning] No technician profile found for user {UserId}", @event.UserId);
            return;
        }

        logger.LogInformation("[Planning] Handling SubscriptionActivated for Technician {TechnicianId}", technicianId);

        var existingCatalog = await catalogRepository.FindByTechnicianIdAsync(
            TechnicianId.From(technicianId));

        if (existingCatalog is not null)
        {
            logger.LogInformation("[Planning] Catalog already exists for Technician {TechnicianId}", technicianId);
            return;
        }

        var newCatalog = ServiceCatalog.Create(TechnicianId.From(technicianId));
        await catalogRepository.AddAsync(newCatalog);
        await unitOfWork.CompleteAsync();

        logger.LogInformation("[Planning] Catalog created for Technician {TechnicianId}", technicianId);
    }
}
