using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IPropertyPortfolioQueryService
{
    Task<PropertyPortfolio?> Handle(GetPortfolioByOwnerIdQuery query);
}