namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;

public record ServiceResource(
    string ServiceId,
    string Name,
    string Description,
    double BasePrice,
    string EstimatedTime,
    string Category,
    bool IsVisible,
    string CreatedBy
);