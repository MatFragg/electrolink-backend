namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

public record UpdateServiceCommand(
    string ServiceId,
    string Name,
    string Description,
    double BasePrice,
    string EstimatedTime,
    string Category,
    bool IsVisible,
    string CreatedBy
);