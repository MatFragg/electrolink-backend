using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Services;

public interface IConsumptionReportQueryService
{
    Task<List<ConsumptionReport>> GetReportsByHomeownerAsync(string homeownerId);
}
