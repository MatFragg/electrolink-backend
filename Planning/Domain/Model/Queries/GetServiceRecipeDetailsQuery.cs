using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

public record GetServiceRecipeDetailsQuery(RecipeId RecipeId, TechnicianId TechnicianId);


