using System;
using System.Collections.Generic;
namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
/*public record ServiceCatalogResource(
    string CatalogId,
    string TechnicianId,
    string ProfileId,
    string Status,
    List<ServiceRecipeDetailResource> Recipes
);*/

public record ServiceCatalogResource(
    string CatalogId,
    string TechnicianId,
    string Status,
    int TotalRecipes,
    IReadOnlyList<ServiceRecipeSummaryResource> Recipes);