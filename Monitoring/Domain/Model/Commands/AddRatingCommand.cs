namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record AddRatingCommand(Guid RequestId, int Score, string Comment, string RaterId, int TechnicianId);
