using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.API.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.API.Application.Internal.QueryServices;

public class ScheduleQueryService(IScheduleRepository scheduleRepository) : IScheduleQueryService
{
    public async Task<IEnumerable<Schedule>> Handle(GetScheduleByTechnicianIdQuery query)
    {
        return await scheduleRepository.ListByTechnicianIdAsync(query.TechnicianId);
    }

    public async Task<IEnumerable<Schedule>> Handle(GetSchedulesByDateQuery query)
    {
        return await scheduleRepository.ListByDateAsync(query.Date);
    }
}