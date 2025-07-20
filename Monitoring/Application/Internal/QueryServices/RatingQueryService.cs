using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hamcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;

namespace Hamcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;

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