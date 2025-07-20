using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;

public interface IScheduleRepository : IBaseRepository<Schedule>
{
    Task<IEnumerable<Schedule>> ListByTechnicianIdAsync(Guid technicianId);
    Task<IEnumerable<Schedule>> ListByDateAsync(DateOnly date);
    Task<Schedule?> FindByIdAsync(string scheduleId);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(Schedule schedule);
}