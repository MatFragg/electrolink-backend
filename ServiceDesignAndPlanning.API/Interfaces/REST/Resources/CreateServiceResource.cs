namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
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