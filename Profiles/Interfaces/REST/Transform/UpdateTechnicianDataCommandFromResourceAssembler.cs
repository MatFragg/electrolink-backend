using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class UpdateTechnicianDataCommandFromResourceAssembler
{
    public static UpdateTechnicianDataCommand ToCommandFromResource(
        UpdateTechnicianDataRequest resource,
        string profileId,
        string userId) =>
        new(
            ProfileId:       profileId,
            UserId:          userId,
            Specialties:     resource.Specialties,
            ExperienceYears: resource.ExperienceYears,
            AboutMe:         resource.AboutMe);
}

