using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class RequestRepository(AppDbContext context)
    : BaseRepository<Request, string>(context), IRequestRepository
{
    public async Task<IEnumerable<Request>> ListByClientIdAsync(Guid clientId)
    {
        return await context.Set<Request>()
            .Where(r => r.ClientId == clientId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Request request)
    {
        context.Set<Request>().Update(request);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Request request)
    {
        context.Set<Request>().Remove(request);
        await Task.CompletedTask;
    }
}
