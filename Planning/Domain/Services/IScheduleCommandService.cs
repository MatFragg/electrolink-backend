using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Services;

public interface IScheduleCommandService
{
    Task<Schedule> CreateAsync(CreateScheduleCommand  createScheduleCommand );
    Task<Schedule?> UpdateAsync(UpdateScheduleCommand command);
    Task<bool> DeleteAsync(DeleteScheduleCommand command);
}
