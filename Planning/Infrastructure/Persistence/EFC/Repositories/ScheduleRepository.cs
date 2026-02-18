using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class ScheduleRepository(AppDbContext context)
    : BaseRepository<Schedule, string>(context), IScheduleRepository
{
    public async Task<Schedule?> FindByIdAsync(string scheduleId)
    {
        return await Context.Set<Schedule>()
            .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);
    }

    public async Task<IEnumerable<Schedule>> ListByTechnicianIdAsync(Guid technicianId)
    {
        return await Context.Set<Schedule>()
            .Where(s => s.TechnicianId == technicianId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> ListByDateAsync(DateOnly date)
    {
        var dateString = date.DayOfWeek.ToString();
        return await Context.Set<Schedule>()
            .Where(s => s.Day == dateString)
            .ToListAsync();
    }

    public async Task UpdateAsync(Schedule schedule)
    {
        Context.Set<Schedule>().Update(schedule);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Schedule schedule)
    {
        Context.Set<Schedule>().Remove(schedule);
        await Context.SaveChangesAsync();
    }
}