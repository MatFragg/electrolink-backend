using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
public record CreateServiceResource(
    string Name,
    string Description,
    decimal BasePrice,
    string EstimatedTime,
    string Category,
    bool IsVisible,
    Guid CreatedBy,
    ServicePolicy Policy,
    ServiceRestriction Restriction,
    List<ServiceTag> Tags,
    List<ServiceComponent> Components
);