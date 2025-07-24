namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record UpdateRatingCommand(Guid RatingId, int Score, string Comment);