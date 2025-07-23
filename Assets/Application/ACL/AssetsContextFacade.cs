using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.ACL;

namespace Hampcoders.Electrolink.API.Assets.Application.ACL;

public class AssetsContextFacade(ITechnicianInventoryCommandService technicianInventoryCommandService,
    ITechnicianInventoryQueryService technicianInventoryQueryService) : IAssetsContextFacade
{
    public async Task<Guid> CreateTechnicianInventory(Guid technicianId)
    {
        // Traducción de primitivo a Comando
        var command = new CreateTechnicianInventoryCommand(technicianId);
        var inventory = await technicianInventoryCommandService.Handle(command);
        return inventory!.Id;
    }

    public async Task<bool> ExistsInventoryForTechnician(Guid technicianProfileId)
    {
        // Traducción de primitivo a Query
        var query = new GetInventoryByTechnicianIdQuery(technicianProfileId);
        var inventory = await technicianInventoryQueryService.Handle(query);
        return inventory != null;
    }
}