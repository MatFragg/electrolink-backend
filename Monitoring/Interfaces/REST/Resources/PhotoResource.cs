namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record PhotoResource(string PhotoId, string PhotoType, string PhotoUrl, DateTime TakenAt, string? Notes);
