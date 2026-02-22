using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Repositories;

public class ProfileRepository(AppDbContext context)
  : BaseRepository<Profile, int>(context), IProfileRepository
{
  public async Task<IEnumerable<Profile>> FindByRoleAsync(EBusinessRole role)
  {
    return await Context.Set<Profile>()
      .Where(p => p.BusinessRole != null && p.BusinessRole.Value == role)
      .ToListAsync();
  }

  public async Task<bool> ExistsByUserIdAsync(UserId userId)
  {
    return await Context.Set<Profile>().AnyAsync(p => p.UserId == userId);
  }
  
  public async Task<bool> DniExistsAsync(Dni dni, ProfileId? excludeProfileId = null)
  {
    return await Context.Set<Profile>()
      .AnyAsync(p => p.PersonalData != null
                     && p.PersonalData.Dni == dni
                     && (excludeProfileId == null || p.ProfileId != excludeProfileId));
  }

  public async Task<bool> IsHomeownerActiveAsync(HomeownerId homeownerId)
  {
    return await Context.Set<Profile>()
      .AnyAsync(p => p.Homeowner != null && p.Homeowner.HomeownerId == homeownerId);
  }

  public async Task<Profile?> FindByUserIdAsync(UserId userId)
  {
    return await Context.Set<Profile>()
      .FirstOrDefaultAsync(p => p.UserId == userId);
  }

  public async Task<Profile?> FindByIdAsync(ProfileId id)
  {
    return await Context.Set<Profile>()
      .FirstOrDefaultAsync(p => p.ProfileId == id);
  }
}
