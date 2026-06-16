namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Queries;

public record GetRecentReadingsWindowQuery(string DeviceId, int WindowMinutes = 60);
