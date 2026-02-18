using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record CreateServiceCommand(
    string Name,
    string Description,
    decimal BasePrice,
    string EstimatedTime,
    string Category,
    bool IsVisible,
    Guid CreatedBy
);