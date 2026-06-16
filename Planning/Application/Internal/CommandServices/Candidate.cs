using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.CommandServices;

public record Candidate(string TechnicianId, double Rating, ServiceRecipe Recipe);
