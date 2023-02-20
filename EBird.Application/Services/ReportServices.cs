using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Report;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;

namespace EBird.Application.Services;
public class ReportServices : IReportServices
{
    private readonly IGenericRepository<ReportEntity> _reportRepository;
    private readonly IMapper _mapper;

    public ReportServices(IGenericRepository<ReportEntity> reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }
    public async Task<List<ReportEntity>> GetAllReport()
    {
        var reports = await _reportRepository.GetAllActiveAsync();
        if (reports == null)
        {
            throw new NotFoundException("Report is not exist");
        }
        return reports;

    }
    public async Task<ReportEntity> GetReportById(Guid id)
    {
        var report = (await _reportRepository.WhereAsync(x => x.Id == id && !x.IsDeleted, new string[] { "CreateBy", "HandleBy" })).FirstOrDefault();
        if (report == null)
        {
            throw new NotFoundException("Report is not exist");
        }
        return report;
    }
    public async Task CreateReport(CreateReport report)
    {
        if (report.CreatedDateTime > report.HandleDateTime)
        {
            throw new BadRequestException("Created date time must be less than handle date time");
        }
        var reportEntity = _mapper.Map<ReportEntity>(report);
        await _reportRepository.CreateAsync(reportEntity);
    }
    public async Task UpdateReport(Guid reportId, UpdateReport updateReport)
    {
        var report = await _reportRepository.GetByIdActiveAsync(reportId);
        if (report == null)
        {
            throw new NotFoundException("Report is not exist");
        }
        _mapper.Map(updateReport, report);
        await _reportRepository.UpdateAsync(report);
    }
    public async Task DeleteReport(Guid reportId)
    {
        await _reportRepository.DeleteSoftAsync(reportId);
    }
}