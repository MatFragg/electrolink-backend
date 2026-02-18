namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record ConfirmServiceRequestCommand(
    Guid RequestId,
    Guid HomeownerId
);

