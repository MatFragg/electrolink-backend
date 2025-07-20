using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Services;

public interface IScheduleQueryService
{
    Task<IEnumerable<Schedule>> Handle(GetScheduleByTechnicianIdQuery query);
    Task<IEnumerable<Schedule>> Handle(GetSchedulesByDateQuery query);
}