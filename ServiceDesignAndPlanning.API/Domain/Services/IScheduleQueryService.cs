using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;

public interface IScheduleQueryService
{
    Task<IEnumerable<Schedule>> Handle(GetScheduleByTechnicianIdQuery query);
    Task<IEnumerable<Schedule>> Handle(GetSchedulesByDateQuery query);
}