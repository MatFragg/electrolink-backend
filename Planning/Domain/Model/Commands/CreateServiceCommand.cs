using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;

public record CreateServiceCommand(
    string ServiceId,
    string Name,
    string Description,
    double BasePrice,
    string EstimatedTime,
    string Category,
    bool IsVisible,
    string CreatedBy,
    ServicePolicy Policy,
    ServiceRestriction Restriction,
    List<ServiceTag> Tags,
    List<ServiceComponent> Components
);