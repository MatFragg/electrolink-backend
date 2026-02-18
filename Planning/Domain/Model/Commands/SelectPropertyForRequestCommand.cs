namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record SelectPropertyForRequestCommand(
    Guid RequestId,
    Guid HomeownerId,
    Guid PropertyId
);

