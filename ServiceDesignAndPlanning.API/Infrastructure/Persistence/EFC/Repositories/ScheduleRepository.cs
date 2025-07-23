using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Infrastructure.Persistence.EFC.Repositories;

public class ScheduleRepository(AppDbContext context)
    : BaseRepository<Schedule>(context), IScheduleRepository
{
    public async Task<IEnumerable<Schedule>> ListByTechnicianIdAsync(Guid technicianId)
    {
        return await context.Set<Schedule>()
            .Where(s => s.TechnicianId == technicianId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> ListByDateAsync(DateOnly date)
    {
        var dateString = date.DayOfWeek.ToString();
        return await context.Set<Schedule>()
            .Where(s => s.Day == dateString)
            .ToListAsync();
    }
    public async Task<Schedule?> FindByIdAsync(string scheduleId)
    {
        return await context.Set<Schedule>().FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);
    }
    public async Task UpdateAsync(Schedule schedule)
    {
        context.Set<Schedule>().Update(schedule);
    }

    public async Task DeleteAsync(Schedule schedule)
    {
        context.Set<Schedule>().Remove(schedule);
    }
}