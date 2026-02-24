using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdatePropertyCommand(string PropertyId, string HomeownerId, Address Address, string RegionName, string RegionCode, string DistrictName, string DistrictUbigeo);