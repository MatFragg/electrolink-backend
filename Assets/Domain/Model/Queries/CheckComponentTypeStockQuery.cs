namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

public record CheckComponentTypeStockQuery(
    string TechnicianId,
    IReadOnlyList<(string ComponentTypeId, int Quantity)> Requirements);