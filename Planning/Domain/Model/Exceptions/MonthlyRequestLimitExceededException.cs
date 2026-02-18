namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class MonthlyRequestLimitExceededException : DomainException
{
    public int Limit { get; }
    public int CurrentUsage { get; }

    public MonthlyRequestLimitExceededException(int limit, int currentUsage) 
        : base($"Monthly request limit exceeded. Limit: {limit}, Current usage: {currentUsage}")
    {
        Limit = limit;
        CurrentUsage = currentUsage;
    }
}