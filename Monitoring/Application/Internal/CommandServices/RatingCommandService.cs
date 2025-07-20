using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Hamcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;

public class RatingCommandService(
    IRatingRepository _repository,
    IServiceOperationRepository _operationRepository,
    IUnitOfWork unitOfWork) : IRatingCommandService
{
    public async Task Handle(AddRatingCommand command)
    {
        var exists = await _operationRepository.FindByIdAsync(command.RequestId);
        if (exists == null) throw new Exception("ServiceOperation not found");

        var existing = await _repository.GetByRequestIdAsync(command.RequestId);
        if (existing != null) return;

        var rating = new Rating(Guid.NewGuid(), command.RequestId, command.Score, command.Comment, command.RaterId, command.TechnicianId);
        await _repository.AddAsync(rating);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(DeleteRatingCommand command)
    {
        var rating = await _repository.FindByIdAsync(command.RatingId);
        if (rating == null)
            throw new KeyNotFoundException("Rating not found");
        _repository.Remove(rating);
        await unitOfWork.CompleteAsync(); 
    }

    public async Task Handle(UpdateRatingCommand command)
    {
        var rating = await _repository.FindByIdAsync(command.RatingId);
        if (rating == null)
            throw new Exception("Rating not found");

        rating.Update(command.Score, command.Comment);
        await unitOfWork.CompleteAsync();
    }
}