using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public class Rating
{
    public Guid RatingId { get; private set; }
    public Guid RequestId { get; private set; }
    public int Score { get; private set; }
    public string Comment { get; private set; }
    public string RaterId { get; private set; }
    public TechnicianId TechnicianId { get; private set; }

    public Rating() {}
    
    public Rating(Guid ratingId, Guid requestId, int score, string comment, string raterId, int technicianId)
    {
        RatingId = ratingId;
        RequestId = requestId;
        Score = score;
        Comment = comment;
        RaterId = raterId;
        TechnicianId = new TechnicianId(technicianId);
    }

    public void Update(int score, string comment)
    {
        Score = score;
        Comment = comment;
    }
}
