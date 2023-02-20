using EBird.Application.Model.Report;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices;
public interface IReportServices
{
    Task<List<ReportEntity>> GetAllReport();
    Task<ReportEntity> GetReportById(Guid id);
    Task CreateReport(CreateReport report);
    Task UpdateReport(Guid reportId, UpdateReport updateReport);
    Task DeleteReport(Guid reportId);
}