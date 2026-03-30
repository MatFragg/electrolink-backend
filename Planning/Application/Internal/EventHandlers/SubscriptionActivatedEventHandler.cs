using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Manejador de evento: Cuando una suscripción se activa en el BC de Subscriptions.
/// Crea un catálogo de servicios vacío para el técnico si no existe.
/// </summary>
public class SubscriptionActivatedEventHandler
{
    private readonly IServiceCatalogRepository _catalogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SubscriptionActivatedEventHandler> _logger;

    public SubscriptionActivatedEventHandler(
        IServiceCatalogRepository catalogRepository,
        IUnitOfWork unitOfWork,
        ILogger<SubscriptionActivatedEventHandler> logger)
    {
        _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(dynamic @event)
    {
        try
        {
            string technicianId = @event.TechnicianId;
            string profileId = @event.ProfileId;

            _logger.LogInformation($"[Planning] Handling SubscriptionActivated for Technician {technicianId}");

            var existingCatalog = await _catalogRepository.FindByTechnicianIdAsync(
                TechnicianId.From(technicianId));

            if (existingCatalog != null)
            {
                _logger.LogInformation($"[Planning] Catalog already exists for Technician {technicianId}");
                return;
            }

            // Crear nuevo catálogo vacío
            var newCatalog = ServiceCatalog.Create(TechnicianId.From(technicianId));
            await _catalogRepository.AddAsync(newCatalog);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"[Planning] Catalog created for Technician {technicianId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning] Error handling SubscriptionActivated event");
            throw;
        }
    }
}

