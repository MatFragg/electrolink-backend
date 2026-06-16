using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Processing.Domain.Repositories;

public interface IAnomalyRecordRepository : IBaseRepository<AnomalyRecord, AnomalyRecordId>
{
    Task<AnomalyRecord?> FindActiveByDeviceAndTypeAsync(string deviceId, EAnomalyType type);
    Task<IEnumerable<AnomalyRecord>> FindActiveByPropertyAsync(string propertyId);
    Task<IEnumerable<AnomalyRecord>> FindByDeviceAsync(string deviceId, int limit = 50);
    Task<IEnumerable<AnomalyRecord>> FindRecentByPropertyAsync(string propertyId, int days = 7);
    Task UpdateAsync(AnomalyRecord anomaly);
}
