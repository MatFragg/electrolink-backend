using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;

public record CreatePropertyCommand(string HomeownerId, Address Address, Region Region, District District);
