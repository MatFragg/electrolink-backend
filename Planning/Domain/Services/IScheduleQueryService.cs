using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IScheduleQueryService
{
    Task<IEnumerable<Schedule>> Handle(GetScheduleByTechnicianIdQuery query);
    Task<IEnumerable<Schedule>> Handle(GetSchedulesByDateQuery query);
}