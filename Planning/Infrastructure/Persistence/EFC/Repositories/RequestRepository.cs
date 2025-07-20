using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Planning.API.Infrastructure.Persistence.EFC.Repositories;

public class RequestRepository(AppDbContext context)
    : BaseRepository<Request>(context), IRequestRepository
{
    public async Task<IEnumerable<Request>> ListByClientIdAsync(Guid clientId)
    {
        return await context.Set<Request>()
            .Where(r => r.ClientId == clientId)
            .ToListAsync();
    }


    public async Task<Request?> FindByIdAsync(string requestId)
    {
        return await context.Set<Request>()
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
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
