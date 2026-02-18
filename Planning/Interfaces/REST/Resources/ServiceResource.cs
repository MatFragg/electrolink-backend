namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record ServiceResource(
    string ServiceId,
    string Name,
    string Description,
    decimal BasePrice,
    string EstimatedTime,
    string Category,
    bool IsVisible,
    Guid CreatedBy
);