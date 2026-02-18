using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IServiceCatalogCommandService
{
    Task<ServiceCatalog?> Handle(CreateServiceCatalogCommand command);
    Task<ServiceRecipe?> Handle(CreateServiceRecipeCommand command);
    Task<ServiceRecipe?> Handle(UpdateServiceRecipeCommand command);
    Task<bool> Handle(DeactivateServiceRecipeCommand command);
    Task<bool> Handle(ReactivateServiceRecipeCommand command);
}

