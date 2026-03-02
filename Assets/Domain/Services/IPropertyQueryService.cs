using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IPropertyQueryService
{
    Task<Property?> Handle(GetPropertyByIdQuery query);
    Task<IEnumerable<Property>> Handle(GetAllPropertiesByOwnerIdQuery query);
    Task<Address?> Handle(GetPropertyAddressQuery query);
}