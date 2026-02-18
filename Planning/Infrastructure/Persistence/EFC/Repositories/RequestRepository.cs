using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class RequestRepository(AppDbContext context)
    : BaseRepository<Request, RequestId>(context), IRequestRepository
{
    public async Task<Request?> FindByIdAsync(RequestId id)
    {
        return await Context.Set<Request>()
            .Include(r => r.Photos)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Request>> ListByClientIdAsync(ClientId clientId)
    {
        if (clientId == null) return Enumerable.Empty<Request>();

        return await Context.Set<Request>()
            .Include(r => r.Photos)
            .Where(r => r.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<int> CountByClientIdAndMonthAsync(ClientId clientId, int year, int month)
    {
        if (clientId == null) return 0;
        if (month < 1 || month > 12) throw new ArgumentOutOfRangeException(nameof(month));

        return await Context.Set<Request>()
            .Where(r =>
                r.ClientId == clientId &&
                r.CreatedDate.HasValue &&
                r.CreatedDate.Value.Year == year &&
                r.CreatedDate.Value.Month == month)
            .CountAsync();
    }

    public async Task UpdateAsync(Request request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        Context.Update(request);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Request request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        Context.Remove(request);
        await Context.SaveChangesAsync();
    }
}