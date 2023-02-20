using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Report;
public class UpdateReport: IMapTo<ReportEntity>
{
    public string Context { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }

}