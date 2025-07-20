namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

public record CreatePropertyResource(AddressResource Address, string RegionName, string DistrictName);
