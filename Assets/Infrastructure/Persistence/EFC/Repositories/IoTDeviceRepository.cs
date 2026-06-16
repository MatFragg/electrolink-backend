using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

public class IoTDeviceRepository : BaseRepository<IoTDevice, IoTDeviceId>, IIoTDeviceRepository
{
    public IoTDeviceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IoTDevice?> FindBySerialNumberAsync(SerialNumber serialNumber)
        => await Context.Set<IoTDevice>().FirstOrDefaultAsync(d => d.SerialNumber == serialNumber);

    public async Task<IEnumerable<IoTDevice>> FindByStatusAsync(EDeviceStatus status)
        => await Context.Set<IoTDevice>().Where(d => d.Status == status).ToListAsync();

    public async Task<IEnumerable<IoTDevice>> FindByPropertyIdAsync(PropertyId propertyId)
        => await Context.Set<IoTDevice>()
            .Where(d => d.AssignedPropertyId != null && d.AssignedPropertyId.Value == propertyId.Value)
            .ToListAsync();

    public async Task<int> CountActiveByOwnerIdAsync(string ownerId)
        => await Context.Set<IoTDevice>()
            .Where(d => d.Status == EDeviceStatus.Installed || d.Status == EDeviceStatus.Maintenance)
            .CountAsync();

    public async Task<IoTDevice?> FindFirstInStockAsync()
        => await Context.Set<IoTDevice>()
            .Where(d => d.Status == EDeviceStatus.InStock)
            .FirstOrDefaultAsync();
}
