using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IScheduleCommandService
{
    Task<Schedule> CreateAsync(CreateScheduleCommand  createScheduleCommand );
    Task<Schedule?> UpdateAsync(UpdateScheduleCommand command);
    Task<bool> DeleteAsync(DeleteScheduleCommand command);
}
