using System;
using System.Collections.Generic;
using System.Linq;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;


/// <summary>
/// Value object representing evaluation categories with ratings.
/// Maps each category to a numeric rating (1-5).
/// </summary>
public record EvaluationCategories
{
    public Dictionary<EEvaluationCategory, int> Categories { get; init; } = new();

    /// <summary>
    /// Adds or updates a category rating.
    /// </summary>
    public void SetCategoryRating(EEvaluationCategory category, int rating)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5");

        Categories[category] = rating;
    }

    /// <summary>
    /// Gets the rating for a specific category.
    /// </summary>
    public int? GetCategoryRating(EEvaluationCategory category)
    {
        return Categories.TryGetValue(category, out var rating) ? rating : null;
    }

    /// <summary>
    /// Calculates the average rating across all categories.
    /// </summary>
    public double GetAverageRating()
    {
        return Categories.Count == 0 ? 0 : Categories.Values.Average();
    }
}

