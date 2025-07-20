namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record RatingResource(Guid RequestId, int Score, string Comment, string RaterId, int TechnicianId);