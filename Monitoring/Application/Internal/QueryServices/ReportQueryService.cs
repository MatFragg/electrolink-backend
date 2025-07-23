using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hamcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

namespace Hamcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;

public class ReportQueryService : IReportQueryService
{
    private readonly IReportRepository reportRepository;
    private readonly IReportPhotoRepository reportPhotoRepository;

    public ReportQueryService(
        IReportRepository reportRepository,
        IReportPhotoRepository reportPhotoRepository)
    {
        this.reportRepository = reportRepository;
        this.reportPhotoRepository = reportPhotoRepository;
    }

    public async Task<IEnumerable<Report>> Handle(GetAllReportsQuery query)
    {
        var reports = await reportRepository.ListAsync();
        var result = new List<Report>();

        foreach (var report in reports)
        {
            var photos = await reportPhotoRepository.GetByReportIdAsync(report.ReportId);
            report.SetPhotos(photos); // necesitas este método en tu aggregate
            result.Add(report);
        }

        return result;
    }

    public async Task<Report?> GetByRequestIdAsync(Guid requestId)
    {
        var report = await reportRepository.GetByRequestIdAsync(requestId);
        if (report is null) return null;

        var photos = await reportPhotoRepository.GetByReportIdAsync(report.ReportId);
        report.SetPhotos(photos);
        return report;
    }

    public async Task<Report?> GetByIdWithPhotosAsync(Guid reportId)
    {
        var report = await reportRepository.FindByIdAsync(reportId);
        if (report is null) return null;

        var photos = await reportPhotoRepository.GetByReportIdAsync(reportId);
        report.SetPhotos(photos);
        return report;
    }
}