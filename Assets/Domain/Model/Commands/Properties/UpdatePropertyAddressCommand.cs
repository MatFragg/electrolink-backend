using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;

public record UpdatePropertyAddressCommand(Guid Id, Address NewAddress);
