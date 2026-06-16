namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Exceptions;

public class MaxWorkPhotosExceededException : Exception
{
    public MaxWorkPhotosExceededException(int maxPhotos, int currentPhotos)
        : base($"Maximum of {maxPhotos} work photos exceeded. Current count: {currentPhotos}.")
    {
    }
}
