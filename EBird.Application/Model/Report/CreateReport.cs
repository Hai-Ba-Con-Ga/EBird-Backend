using System.ComponentModel.DataAnnotations;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Report;
public class CreateReport : IMapTo<ReportEntity>
{
    [Required]
    public string Context { get; set; }
    [Required]
    public string Title { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public DateTime HandleDateTime { get; set; } = DateTime.Now;
    public Guid CreateById { get; set; } 
    public Guid HandleById { get; set; }
}