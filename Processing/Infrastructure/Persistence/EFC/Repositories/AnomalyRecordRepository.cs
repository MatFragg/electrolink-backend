using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Processing.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Processing.Infrastructure.Persistence.EFC.Repositories;

public class AnomalyRecordRepository(AppDbContext context)
    : BaseRepository<AnomalyRecord, AnomalyRecordId>(context), IAnomalyRecordRepository
{
    public async Task<AnomalyRecord?> FindActiveByDeviceAndTypeAsync(string deviceId, EAnomalyType type)
        => await Context.Set<AnomalyRecord>()
            .FirstOrDefaultAsync(a =>
                a.DeviceId    == DeviceId.From(deviceId) &&
                a.AnomalyType == type &&
                a.Status      == EAnomalyStatus.Active);

    public async Task<IEnumerable<AnomalyRecord>> FindActiveByPropertyAsync(string propertyId)
        => await Context.Set<AnomalyRecord>()
            .Where(a => a.PropertyId == PropertyId.From(propertyId)
                     && a.Status == EAnomalyStatus.Active)
            .OrderByDescending(a => a.DetectedAt)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<AnomalyRecord>> FindByDeviceAsync(string deviceId, int limit = 50)
        => await Context.Set<AnomalyRecord>()
            .Where(a => a.DeviceId == DeviceId.From(deviceId))
            .OrderByDescending(a => a.DetectedAt)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<AnomalyRecord>> FindRecentByPropertyAsync(string propertyId, int days = 7)
        => await Context.Set<AnomalyRecord>()
            .Where(a => a.PropertyId == PropertyId.From(propertyId)
                     && a.DetectedAt >= DateTime.UtcNow.AddDays(-days))
            .OrderByDescending(a => a.DetectedAt)
            .AsNoTracking()
            .ToListAsync();

    public async Task UpdateAsync(AnomalyRecord anomaly)
        => Context.Set<AnomalyRecord>().Update(anomaly);
}
