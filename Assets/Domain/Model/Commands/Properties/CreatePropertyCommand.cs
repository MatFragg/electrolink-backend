using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;

public record CreatePropertyCommand(OwnerId OwnerId, Address Address, Region Region, District District);
