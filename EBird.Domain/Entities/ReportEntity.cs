using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EBird.Domain.Common;

namespace EBird.Domain.Entities;
[Table("Report")]
public class ReportEntity : BaseEntity
{
    [Column("Context", TypeName = "text")]
    public string Context { get; set; }
    [Column("Title", TypeName = "nvarchar")]
    [MaxLength(100)]
    public string Title { get; set; }

    [Column("Status", TypeName = "nvarchar")]
    [MaxLength(20)]
    public string Status { get; set; }

    [Column("CreatedDateTime", TypeName = "datetime")]
    public DateTime CreatedDateTime { get; set; }

    [Column("HandleDateTime", TypeName = "datetime")]
    public DateTime HandleDateTime { get; set; }
    
    public Guid CreateById { get; set; }
    public AccountEntity CreateBy { get; set; }
    public Guid HandleById { get; set; }
    public AccountEntity HandleBy { get; set; }
}