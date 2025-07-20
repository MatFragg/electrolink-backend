namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

public record GetComponentsByIdsQuery(IEnumerable<Guid> Ids);
