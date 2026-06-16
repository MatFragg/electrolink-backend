using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Exceptions;

public class MaxWorkPhotosPerTypeExceededException : Exception
{
    public MaxWorkPhotosPerTypeExceededException(EPhotoType photoType, int maxPerType, int currentCount)
        : base($"Maximum of {maxPerType} photos for type {photoType} exceeded. Current count: {currentCount}.")
    {
    }
}
