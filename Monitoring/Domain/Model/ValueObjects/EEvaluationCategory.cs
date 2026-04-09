using System;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Enumeration of evaluation categories for service evaluations.
/// Used to categorize different aspects of the service experience.
/// </summary>
public enum EEvaluationCategory
{
    /// <summary>Professionalism and behavior of the technician</summary>
    Professionalism = 1,

    /// <summary>Quality of the work performed</summary>
    WorkQuality = 2,

    /// <summary>Punctuality and time management</summary>
    Punctuality = 3,

    /// <summary>Cleanliness and respect for property</summary>
    Cleanliness = 4,

    /// <summary>Communication and customer service</summary>
    Communication = 5,

    /// <summary>Knowledge and expertise demonstrated</summary>
    Knowledge = 6,

    /// <summary>Value for money</summary>
    ValueForMoney = 7
}

