using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;

public class ReportCommandService(IReportRepository _repository, 
    IServiceOperationRepository _operationRepository, IReportPhotoRepository _reportPhotoRepository,
    IUnitOfWork unitOfWork) : IReportCommandService
{
    
    public async Task Handle(AddReportCommand command)
    {
        // 🔐 Validar si existe el ServiceOperation
        var operation = await _operationRepository.FindByIdAsync(command.RequestId);
        if (operation == null)
            throw new Exception("ServiceOperation not found");

        var existing = await _repository.GetByRequestIdAsync(command.RequestId);
        if (existing != null) return;

        var report = new Report(Guid.NewGuid(), command.RequestId, command.Description, DateTime.UtcNow);
        await _repository.AddAsync(report);
        await unitOfWork.CompleteAsync();
    }
    public async Task Handle(AddReportPhotoCommand command)
    {
        var report = await _repository.FindByIdAsync(command.ReportId);
        if (report == null) throw new Exception("Report not found");
        
        var existingPhotos = await _reportPhotoRepository.GetByReportIdAsync(command.ReportId);
        var alreadyExists = existingPhotos.Any(p => p.Url == command.Url && p.Type == command.Type);
        if (alreadyExists) return;
        
        var photo = new ReportPhoto(command.ReportId, command.Url, command.Type);
        await _reportPhotoRepository.AddAsync(photo);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InvalidOperationException("Concurrency conflict occurred while saving changes.", ex);
        }
    }

    public async Task Handle(DeleteReportCommand command)
    {
        var report = await _repository.FindByIdAsync(command.ReportId);
        if (report == null)
            throw new KeyNotFoundException("Rating not found");
        _repository.Remove(report);
        await unitOfWork.CompleteAsync(); 
    }
}
