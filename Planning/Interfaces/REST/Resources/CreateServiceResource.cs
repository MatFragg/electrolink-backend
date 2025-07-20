using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;
using Planning.API.Domain.Model.ValueObjects;
public record CreateServiceResource(
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