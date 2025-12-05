using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IScheduleRepository : IBaseRepository<Schedule, string>
{
    Task<IEnumerable<Schedule>> ListByTechnicianIdAsync(Guid technicianId);
    Task<IEnumerable<Schedule>> ListByDateAsync(DateOnly date);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(Schedule schedule);
}