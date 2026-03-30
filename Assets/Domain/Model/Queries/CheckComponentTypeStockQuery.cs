namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

public record CheckComponentTypeStockQuery(
    string TechnicianId,
    IReadOnlyList<(string ComponentTypeId, int Quantity)> Requirements);