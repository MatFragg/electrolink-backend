namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record PricingResource(
    decimal MaterialsEstimate,
    decimal LaborCost,
    decimal TotalPrice,
    string  Currency);