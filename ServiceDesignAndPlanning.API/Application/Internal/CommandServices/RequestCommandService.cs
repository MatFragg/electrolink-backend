using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.Internal.CommandServices;

public class RequestCommandService(
    IRequestRepository requestRepository,
    IUnitOfWork unitOfWork
) : IRequestCommandService
{
    public async Task<Request> Handle(CreateRequestCommand command)
    {
        var request = new Request(command); // ← ID se genera internamente
        await requestRepository.AddAsync(request);
        await unitOfWork.CompleteAsync();
        return request;
    }

    public async Task<Request?> UpdateAsync(UpdateRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId);
        if (request is null) return null;

        request.UpdateScheduledDate(command.ScheduledDate);
        request.AssignTechnician(command.TechnicianId);
        request.ProblemDescription = command.ProblemDescription;

        await requestRepository.UpdateAsync(request);
        await unitOfWork.CompleteAsync();

        return request;
    }

    public async Task<bool> DeleteAsync(DeleteRequestCommand command)
    {
        var request = await requestRepository.FindByIdAsync(command.RequestId);
        if (request is null) return false;

        await requestRepository.DeleteAsync(request);
        await unitOfWork.CompleteAsync();
        return true;
    }
}