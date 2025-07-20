using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record UpdatePropertyResource(Guid OwnerId, Address Address, string RegionName, string RegionCode, string DistrictName, string DistrictUbigeo);