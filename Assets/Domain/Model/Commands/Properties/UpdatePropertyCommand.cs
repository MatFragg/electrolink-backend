using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;

public record UpdatePropertyCommand(Guid Id, Guid OwnerId, Address Address,  string RegionName, 
    string RegionCode,
    string DistrictName,
    string DistrictUbigeo);