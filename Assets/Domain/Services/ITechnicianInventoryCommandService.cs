using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface ITechnicianInventoryCommandService
{
    Task<TechnicianInventory?> Handle(CreateTechnicianInventoryCommand command);
    Task<TechnicianInventory?> Handle(AddStockToInventoryCommand command);
    Task<TechnicianInventory?> Handle(IncreaseStockCommand command);
    Task<TechnicianInventory?> Handle(DecreaseStockCommand command);
    Task<TechnicianInventory?> Handle(UpdateComponentStockCommand command);
    Task<bool> Handle(RemoveComponentStockCommand command);
    Task<TechnicianInventory?> Handle(AdjustTechnicianInventoryCommand command);
}