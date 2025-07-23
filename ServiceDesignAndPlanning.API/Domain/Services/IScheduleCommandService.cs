using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;

public interface IScheduleCommandService
{
    Task<Schedule> CreateAsync(CreateScheduleCommand  createScheduleCommand );
    Task<Schedule?> UpdateAsync(UpdateScheduleCommand command);
    Task<bool> DeleteAsync(DeleteScheduleCommand command);
}
