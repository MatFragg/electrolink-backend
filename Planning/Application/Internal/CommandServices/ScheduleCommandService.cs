using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.API.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.API.Application.Internal.CommandServices;

public class ScheduleCommandService(
    IScheduleRepository scheduleRepository,
    IUnitOfWork unitOfWork
) : IScheduleCommandService
{
    public async Task<Schedule> CreateAsync(CreateScheduleCommand cmd)
    {
        var schedule = new Schedule(cmd.ScheduleId, cmd.TechnicianId, cmd.Day, cmd.StartTime, cmd.EndTime);
        await scheduleRepository.AddAsync(schedule);
        await unitOfWork.CompleteAsync();
        return schedule;
    }

    public async Task<Schedule?> UpdateAsync(UpdateScheduleCommand command)
    {
        var schedule = await scheduleRepository.FindByIdAsync(command.ScheduleId);
        if (schedule is null) return null;

        schedule.UpdateSchedule(command.Day, command.StartTime, command.EndTime);

        await scheduleRepository.UpdateAsync(schedule);
        await unitOfWork.CompleteAsync();

        return schedule;
    }


    public async Task<bool> DeleteAsync(DeleteScheduleCommand command)
    {
        var schedule = await scheduleRepository.FindByIdAsync(command.ScheduleId);
        if (schedule is null) return false;

        await scheduleRepository.DeleteAsync(schedule);
        await unitOfWork.CompleteAsync();

        return true;
    }
    
}
