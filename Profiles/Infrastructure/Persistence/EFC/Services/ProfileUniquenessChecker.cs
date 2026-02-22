using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Services;

public class ProfileUniquenessChecker(AppDbContext context) : IProfileUniquenessChecker
{
    public void EnsureDniIsUnique(Dni dni, ProfileId excludedProfileId)
    {
        var exists = context.Set<Profile>()
            .Any(p => p.PersonalData!.Dni == dni
                      && p.ProfileId != excludedProfileId);

        if (exists)
            throw new DniAlreadyInUseException(dni);
    }
}