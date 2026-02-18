namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record CancelServiceRequestCommand(
    Guid RequestId,
    Guid HomeownerId,
    string Reason
);

