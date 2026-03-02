using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IPropertyPortfolioCommandService
{
    Task<PropertyPortfolio?> Handle(CreatePropertyPortfolioCommand command);
    Task<PropertyPortfolio?> Handle(AddPropertyToPortfolioCommand command);
    Task<bool> Handle(RemovePropertyFromPortfolioCommand command);
}
