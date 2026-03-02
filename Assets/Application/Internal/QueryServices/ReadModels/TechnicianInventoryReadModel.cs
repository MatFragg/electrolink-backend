using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices.ReadModels;

public record TechnicianInventoryReadModel(
    TechnicianInventory Inventory,
    IReadOnlyDictionary<string, string> ComponentNames
);