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
  public async Task<Profile?> FindByEmailAsync(Email email)
  {
    return await Context.Set<Profile>()
      .FirstOrDefaultAsync(p => p.PersonalData != null && p.PersonalData.Email.Value == email.Value);
  }

  public async Task<IEnumerable<Profile>> FindByRoleAsync(EBusinessRole role)
  {
    return await Context.Set<Profile>()
      .Where(p => p.BusinessRole != null && p.BusinessRole.Value == role)
      .ToListAsync();
  }

  public async Task<bool> ExistsByUserIdAsync(UserId userId)
  {
    return await Context.Set<Profile>().AnyAsync(p => p.UserId.Value == userId.Value);
  }

  public async Task<bool> EmailExistsAsync(Email email, ProfileId? excludeProfileId = null)
  {
    return await Context.Set<Profile>()
      .AnyAsync(p => p.PersonalData != null
                     && p.PersonalData.Email.Value == email.Value
                     && (excludeProfileId == null || p.ProfileId.Value != excludeProfileId.Value));
  }

  public async Task<bool> DniExistsAsync(Dni dni, ProfileId? excludeProfileId = null)
  {
    return await Context.Set<Profile>()
      .AnyAsync(p => p.PersonalData != null
                     && p.PersonalData.Dni.Value == dni.Value
                     && (excludeProfileId == null || p.ProfileId.Value != excludeProfileId.Value));
  }

  public async Task<bool> IsHomeownerActiveAsync(HomeownerId homeownerId)
  {
    return await Context.Set<Profile>()
      .AnyAsync(p => p.Homeowner != null && p.Homeowner.HomeownerId.Value == homeownerId.Value);
  }

  public async Task<bool> ExistsByEmailAsync(Email email)
  {
    return await Context.Set<Profile>().AnyAsync(p => p.PersonalData != null && p.PersonalData.Email.Value == email.Value);
  }

  public async Task<Profile?> FindByUserIdAsync(UserId userId)
  {
    return await Context.Set<Profile>()
      .FirstOrDefaultAsync(p => p.UserId.Value == userId.Value);
  }

  public async Task<Profile?> FindByIdAsync(ProfileId id)
  {
    return await Context.Set<Profile>()
      .FirstOrDefaultAsync(p => p.ProfileId.Value == id.Value);
  }
}
