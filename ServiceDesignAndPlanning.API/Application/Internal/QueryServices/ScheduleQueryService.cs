using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Queries;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.Internal.QueryServices;

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