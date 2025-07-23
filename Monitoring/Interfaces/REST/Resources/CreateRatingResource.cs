namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record CreateRatingResource(int Score, string Comment, string RaterId, int TechnicianId);
