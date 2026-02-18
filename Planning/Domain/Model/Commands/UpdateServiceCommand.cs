namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record UpdateServiceCommand(
    Guid ServiceId,
    string Name,
    string Description,
    decimal BasePrice,
    string EstimatedTime,
    string Category,
    bool IsVisible
);