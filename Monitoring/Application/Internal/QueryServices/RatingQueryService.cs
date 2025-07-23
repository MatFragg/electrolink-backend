using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;

public class RatingQueryService : IRatingQueryService
{
    private readonly IRatingRepository _repository;

    public RatingQueryService(IRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Rating?> GetByRequestIdAsync(Guid requestId)
    {
        return await _repository.GetByRequestIdAsync(requestId);
    }
    

    public async Task<IEnumerable<Rating>> GetByTechnicianIdAsync(int technicianId)
    {
        return await _repository.GetByTechnicianIdAsync(technicianId);
    }
    
    public async Task<IEnumerable<Rating>> Handle(GetAllRatingsQuery query) =>
        await _repository.ListAsync();
}