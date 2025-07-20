using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record CreatePropertyCommand(OwnerId OwnerId, Address Address, Region Region, District District);
